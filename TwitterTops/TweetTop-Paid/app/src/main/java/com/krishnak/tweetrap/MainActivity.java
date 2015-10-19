package com.droid108.tweetrap;

import android.os.Bundle;
import android.os.Handler;
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

import com.droid108.tweetrap.fragments.AutoFragment;
import com.droid108.tweetrap.fragments.BusinessFragment;
import com.droid108.tweetrap.fragments.JokesFragment;
import com.droid108.tweetrap.fragments.LoveFragment;
import com.droid108.tweetrap.fragments.ScienceFragment;
import com.droid108.tweetrap.fragments.TechnologyFragment;

public class MainActivity extends AppCompatActivity {
    NavigationView nvDrawer;
    private DrawerLayout mDrawer;
    private Toolbar toolbar;
    private Handler mHandler;       // Handler to display the ad on the UI thread
    private Runnable displayAd;

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.activity_main);
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
            int fragmentId = 0;
            String fragId = com.droid108.tweetrap.Helpers.SPF.GetSharedPreference(R.string.spf_selected_fragment, this);
            if (fragId != null && !fragId.equals("")) {
                fragmentId = Integer.parseInt(com.droid108.tweetrap.Helpers.SPF.GetSharedPreference(R.string.spf_selected_fragment, this));
            }

            loadFragment(fragmentId);
        }
    }

    public void loadFragment(int menuItemId) {
        // Create a new fragment and specify the planet to show based on
        // position
        Fragment fragment = null;
        Class fragmentClass;
        int selectedFragment = 0;
        switch (menuItemId) {
            case R.id.nav_love_fragment:
                fragmentClass = LoveFragment.class;
                selectedFragment = 1;
                break;
            case R.id.nav_jokes_fragment:
                fragmentClass = JokesFragment.class;
                selectedFragment = 2;
                break;
            case R.id.nav_sports_fragment:
                fragmentClass = com.droid108.tweetrap.fragments.SportsFragment.class;
                selectedFragment = 4;
                break;
            case R.id.nav_tech_fragment:
                fragmentClass = TechnologyFragment.class;
                selectedFragment = 8;
                break;
            case R.id.nav_facts_fragment:
                fragmentClass = com.droid108.tweetrap.fragments.FactsFragment.class;
                selectedFragment = 5;
                break;
            case R.id.nav_latest_news_fragment:
                fragmentClass = com.droid108.tweetrap.fragments.NewsFragment.class;
                selectedFragment = 3;
                break;
            case R.id.nav_science_fragment:
                fragmentClass = ScienceFragment.class;
                selectedFragment = 7;
                break;
            case R.id.nav_business_fragment:
                fragmentClass = BusinessFragment.class;
                selectedFragment = 6;
                break;
            case R.id.nav_auto_fragment:
                fragmentClass = AutoFragment.class;
                selectedFragment = 9;
                break;
            default:
                fragmentClass = LoveFragment.class;
                selectedFragment = 1;
                break;
        }
        com.droid108.tweetrap.Helpers.SPF.SetSharedPreference(this, R.string.spf_selected_fragment, String.valueOf(selectedFragment));
        try {
            fragment = (Fragment) fragmentClass.newInstance();
        } catch (Exception e) {
            e.printStackTrace();
        }
        // Insert the fragment by replacing any existing fragment
        FragmentManager fragmentManager = getSupportFragmentManager();
        fragmentManager.beginTransaction().replace(R.id.flContent, fragment).commit();

        mDrawer.closeDrawers();

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
        int selectedFragment = 0;
        switch (menuItem.getItemId()) {
            case R.id.nav_love_fragment:
                fragmentClass = LoveFragment.class;
                selectedFragment = 1;
                break;
            case R.id.nav_jokes_fragment:
                fragmentClass = JokesFragment.class;
                selectedFragment = 2;
                break;
            case R.id.nav_sports_fragment:
                fragmentClass = com.droid108.tweetrap.fragments.SportsFragment.class;
                selectedFragment = 4;
                break;
            case R.id.nav_tech_fragment:
                fragmentClass = TechnologyFragment.class;
                selectedFragment = 8;
                break;
            case R.id.nav_facts_fragment:
                fragmentClass = com.droid108.tweetrap.fragments.FactsFragment.class;
                selectedFragment = 5;
                break;
            case R.id.nav_latest_news_fragment:
                fragmentClass = com.droid108.tweetrap.fragments.NewsFragment.class;
                selectedFragment = 3;
                break;
            case R.id.nav_science_fragment:
                fragmentClass = ScienceFragment.class;
                selectedFragment = 7;
                break;
            case R.id.nav_business_fragment:
                fragmentClass = BusinessFragment.class;
                selectedFragment = 6;
                break;
            case R.id.nav_auto_fragment:
                fragmentClass = AutoFragment.class;
                selectedFragment = 9;
                break;
            default:
                fragmentClass = LoveFragment.class;
                selectedFragment = 1;
                break;
        }
        com.droid108.tweetrap.Helpers.SPF.SetSharedPreference(this, R.string.spf_selected_fragment, String.valueOf(selectedFragment));
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
        if (menuItem.getItemId() != R.id.nav_open_soource) {
            setTitle(menuItem.getTitle());
        }
        if (menuItem.getItemId() == R.id.nav_open_soource) {
            com.droid108.tweetrap.fragments.LicenseFragment.displayLicensesFragment(getSupportFragmentManager(), true);
        }
        mDrawer.closeDrawers();

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
