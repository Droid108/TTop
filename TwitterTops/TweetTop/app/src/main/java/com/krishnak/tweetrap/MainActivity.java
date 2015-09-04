package com.droid108.tweetrap;

import android.graphics.drawable.ColorDrawable;
import android.os.Bundle;
import android.os.Handler;
import android.os.Looper;
import android.support.design.widget.NavigationView;
import android.support.v4.app.Fragment;
import android.support.v4.app.FragmentManager;
import android.support.v4.view.GravityCompat;
import android.support.v4.widget.DrawerLayout;
import android.support.v7.app.ActionBar;
import android.support.v7.app.AppCompatActivity;
import android.support.v7.widget.Toolbar;
import android.view.Menu;
import android.view.MenuItem;

import com.droid108.tweetrap.fragments.JokesFragment;
import com.droid108.tweetrap.fragments.LoveFragment;
import com.google.android.gms.ads.AdListener;
import com.google.android.gms.ads.AdRequest;
import com.google.android.gms.ads.InterstitialAd;
import com.droid108.tweetrap.fragments.AutoFragment;
import com.droid108.tweetrap.fragments.BusinessFragment;
import com.droid108.tweetrap.fragments.ScienceFragment;
import com.droid108.tweetrap.fragments.TechnologyFragment;

public class MainActivity extends AppCompatActivity {
    private DrawerLayout mDrawer;
    private Toolbar toolbar;
    NavigationView nvDrawer;
    private InterstitialAd adView;  // The ad
    private Handler mHandler;       // Handler to display the ad on the UI thread
    private Runnable displayAd;

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.activity_main);
        adView = new InterstitialAd(this);
        adView.setAdUnitId("ca-app-pub-5466072481479588/8699094755");
        adView.setAdListener(new AdListener() {
            @Override
            public void onAdLoaded() {
                super.onAdLoaded();

            }

            @Override
            public void onAdOpened() {
                super.onAdOpened();
            }

            @Override
            public void onAdLeftApplication() {
                super.onAdLeftApplication();
            }

            @Override
            public void onAdFailedToLoad(int errorCode) {
                super.onAdFailedToLoad(errorCode);
            }

            @Override
            public void onAdClosed() {
                super.onAdClosed();
                loadAd();
            }
        });
        mHandler = new Handler(Looper.getMainLooper());
        displayAd = new Runnable() {
            public void run() {
                runOnUiThread(new Runnable() {
                    public void run() {
                        if (adView.isLoaded()) {
                            adView.show();
                        }
                    }
                });
            }
        };
        loadAd();
        // Set a Toolbar to replace the ActionBar.
        toolbar = (Toolbar) findViewById(R.id.toolbar);
        setSupportActionBar(toolbar);
        // Find our drawer view
        mDrawer = (DrawerLayout) findViewById(R.id.drawer_layout);
        // Set the menu icon instead of the launcher icon.
        final ActionBar ab = getSupportActionBar();
        ab.setHomeAsUpIndicator(R.drawable.ic_action_nd);
        ab.setDisplayHomeAsUpEnabled(true);
        // Find our drawer view
        nvDrawer = (NavigationView) findViewById(R.id.nvView);
        nvDrawer.setSelected(true);
        setupDrawerContent(nvDrawer);
        if (savedInstanceState == null) {
            getSupportFragmentManager().beginTransaction().replace(R.id.flContent, new LoveFragment()).commit();
        }
    }

    void loadAd() {
        AdRequest adRequest = new AdRequest.Builder()
                //.addTestDevice(AdRequest.DEVICE_ID_EMULATOR)
                .build();
        // Load the adView object witht he request
        adView.loadAd(adRequest);
    }

    //Call displayInterstitial() once you are ready to display the ad.
    public void displayInterstitial() {
        mHandler.postDelayed(displayAd, 1);
    }

    private void setupDrawerContent(NavigationView navigationView) {
        navigationView.setNavigationItemSelectedListener(
                new NavigationView.OnNavigationItemSelectedListener() {
                    @Override
                    public boolean onNavigationItemSelected(MenuItem menuItem) {
                        selectDrawerItem(menuItem);
                        return true;
                    }
                });
    }

    public void selectDrawerItem(MenuItem menuItem) {
        // Create a new fragment and specify the planet to show based on
        // position
        Fragment fragment = null;
        Class fragmentClass;
        switch (menuItem.getItemId()) {
            case R.id.nav_love_fragment:
                fragmentClass = LoveFragment.class;
                break;
            case R.id.nav_jokes_fragment:
                fragmentClass = JokesFragment.class;
                break;
            case R.id.nav_sports_fragment:
                fragmentClass = com.droid108.tweetrap.fragments.SportsFragment.class;
                break;
            case R.id.nav_tech_fragment:
                fragmentClass = TechnologyFragment.class;
                break;
            case R.id.nav_facts_fragment:
                fragmentClass = com.droid108.tweetrap.fragments.FactsFragment.class;
                break;
            case R.id.nav_latest_news_fragment:
                fragmentClass = com.droid108.tweetrap.fragments.NewsFragment.class;
                break;
            case R.id.nav_science_fragment:
                fragmentClass = ScienceFragment.class;
                break;
            case R.id.nav_business_fragment:
                fragmentClass = BusinessFragment.class;
                break;
            case R.id.nav_auto_fragment:
                fragmentClass = AutoFragment.class;
                break;
            default:
                fragmentClass = LoveFragment.class;
        }

        try {
            fragment = (Fragment) fragmentClass.newInstance();
        } catch (Exception e) {
            e.printStackTrace();
        }

        // Insert the fragment by replacing any existing fragment
        FragmentManager fragmentManager = getSupportFragmentManager();
        fragmentManager.beginTransaction().replace(R.id.flContent, fragment).commit();

        // Highlight the selected item, update the title, and close the drawer
        menuItem.setChecked(true);
        if(menuItem.getItemId() != R.id.nav_open_soource) {
            setTitle(menuItem.getTitle());
        }
        if (menuItem.getItemId() == R.id.nav_open_soource) {
            com.droid108.tweetrap.fragments.LicenseFragment.displayLicensesFragment(getSupportFragmentManager(), true);
        }
        mDrawer.closeDrawers();
        if (adView.isLoaded()) {
            adView.show();
        }
    }

    @Override
    public boolean onCreateOptionsMenu(Menu menu) {
        // Inflate the menu; this adds items to the action bar if it is present.
        getMenuInflater().inflate(R.menu.menu_main, menu);
        return true;
    }

    @Override
    public boolean onOptionsItemSelected(MenuItem item) {
        // Handle action bar item clicks here. The action bar will
        // automatically handle clicks on the Home/Up button, so long
        // as you specify a parent activity in AndroidManifest.xml.
        switch (item.getItemId()) {
            case android.R.id.home:
                mDrawer.openDrawer(GravityCompat.START);
                return true;
        }
        return super.onOptionsItemSelected(item);
    }

    @Override
    protected void onPostCreate(Bundle savedInstanceState) {
        super.onPostCreate(savedInstanceState);
    }

}
