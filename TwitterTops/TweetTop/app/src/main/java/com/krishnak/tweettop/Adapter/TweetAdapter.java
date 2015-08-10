package com.krishnak.tweettop.Adapter;

import android.app.Dialog;
import android.app.ProgressDialog;
import android.content.Context;
import android.content.SharedPreferences;
import android.graphics.Bitmap;
import android.net.ParseException;
import android.net.Uri;
import android.os.AsyncTask;
import android.text.util.Linkify;
import android.util.Log;
import android.util.Patterns;
import android.view.LayoutInflater;
import android.view.View;
import android.view.ViewGroup;
import android.view.Window;
import android.webkit.WebView;
import android.webkit.WebViewClient;
import android.widget.BaseAdapter;
import android.widget.ImageView;
import android.widget.TextView;
import android.widget.Toast;

import com.afollestad.materialdialogs.MaterialDialog;
import com.krishnak.tweettop.R;
import com.nostra13.universalimageloader.cache.memory.impl.WeakMemoryCache;
import com.nostra13.universalimageloader.core.DisplayImageOptions;
import com.nostra13.universalimageloader.core.ImageLoader;
import com.nostra13.universalimageloader.core.ImageLoaderConfiguration;
import com.nostra13.universalimageloader.core.assist.ImageScaleType;
import com.nostra13.universalimageloader.core.display.FadeInBitmapDisplayer;

import org.json.JSONException;
import org.json.JSONObject;

import java.text.SimpleDateFormat;
import java.util.ArrayList;
import java.util.Calendar;
import java.util.Date;
import java.util.TimeZone;
import java.util.regex.Matcher;
import java.util.regex.Pattern;

import twitter4j.Twitter;
import twitter4j.TwitterException;
import twitter4j.TwitterFactory;
import twitter4j.User;
import twitter4j.auth.AccessToken;
import twitter4j.auth.RequestToken;

/**
 * Created by SupportPedia on 11-04-2015.
 */
public class TweetAdapter extends BaseAdapter {
    Context context;
    ArrayList<JSONObject> list;
    Twitter twitter;
    RequestToken requestToken = null;
    AccessToken accessToken;
    String oauth_url,oauth_verifier,profile_url;
    Dialog auth_dialog;
    WebView web;
    SharedPreferences pref;
    ProgressDialog progress;
    Bitmap bitmap;

    public TweetAdapter(Context context, ArrayList<JSONObject> list) {
        this.context = context;
        this.list = list;
        DisplayImageOptions defaultOptions = new DisplayImageOptions.Builder()
                .cacheOnDisc(true).cacheInMemory(true)
                .imageScaleType(ImageScaleType.EXACTLY)
                .displayer(new FadeInBitmapDisplayer(300)).build();
        ImageLoaderConfiguration config = new ImageLoaderConfiguration.Builder(
                context)
                .defaultDisplayImageOptions(defaultOptions)
                .memoryCache(new WeakMemoryCache())
                .discCacheSize(100 * 1024 * 1024).build();
        ImageLoader.getInstance().init(config);
    }

    @Override
    public int getCount() {
        // TODO Auto-generated method stub
        return list.size();
    }

    @Override
    public Object getItem(int position) {
        return list.get(0);
    }

    @Override
    public long getItemId(int position) {
        return position;
    }

    @Override
    public View getView(int position, View convertView, ViewGroup parent) {
        // TODO Auto-generated method stub
        ViewHolder holder;
        LayoutInflater inflater;
        if (convertView == null) {
            inflater = (LayoutInflater) context.getSystemService(Context.LAYOUT_INFLATER_SERVICE);
            convertView = inflater.inflate(R.layout.twit_list_item, null);
            holder = new ViewHolder();
            holder.imgPic = (ImageView) convertView.findViewById(R.id.icon);
            holder.txtTweet = (TextView) convertView.findViewById(R.id.title);
            holder.txtDesc = (TextView) convertView.findViewById(R.id.desc);
            holder.txtTweet_Id = (TextView) convertView.findViewById(R.id.title_id);
            holder.txtTime = (TextView) convertView.findViewById(R.id.textTime);
            holder.txtRetweetCount = (TextView) convertView.findViewById(R.id.txtRetweetCount);
            holder.txtFavCount = (TextView) convertView.findViewById(R.id.txtFavCount);
            holder.imgFav = (ImageView) convertView.findViewById(R.id.imgFav);
            holder.imgShare = (ImageView) convertView.findViewById(R.id.imgShare);
            holder.imgRetweet = (ImageView) convertView.findViewById(R.id.imgRetweet);
            convertView.setTag(holder);
        } else {
            holder = (ViewHolder) convertView.getTag();
        }
        holder.imgFav.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View view) {
                new MaterialDialog.Builder(context)
                        .title(R.string.Dialog_share_title)
                        .content(R.string.Dialog_share_body)
                        .positiveText(R.string.options_continue)
                        .negativeText(R.string.options_cancel)
                        .show();
            }
        });

        holder.imgRetweet.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View view) {
                new MaterialDialog.Builder(context)
                        .title(R.string.Dialog_share_title)
                        .content(R.string.Dialog_share_body)
                        .positiveText(R.string.options_continue)
                        .negativeText(R.string.options_cancel)
                        .callback(new MaterialDialog.ButtonCallback() {
                            @Override
                            public void onPositive(MaterialDialog dialog) {
                                goToLoginScreen();
                            }
                        })
                        .show();
            }
        });

        holder.imgShare.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View view) {
                new MaterialDialog.Builder(context)
                        .title(R.string.Dialog_share_title)
                        .content(R.string.Dialog_share_body)
                        .positiveText(R.string.options_continue)
                        .negativeText(R.string.options_cancel)
                        .show();
            }
        });
        ImageLoader imageLoader = ImageLoader.getInstance();
        DisplayImageOptions options = new DisplayImageOptions.Builder().cacheInMemory(true)
                .cacheOnDisc(true).resetViewBeforeLoading(true).build();
        ImageView imageView = (ImageView) holder.imgPic;
        imageView.setImageResource(R.drawable.ic_launcher);
        JSONObject object = list.get(position);

        try {
            holder.txtTweet.setText(object.getString("name"));
            holder.txtDesc.setText(android.text.Html.fromHtml(object.getString("text")));
            String img_profile_pic = object.getString("ProfileUrl");
            if (img_profile_pic.indexOf("normal") > 0) {
                img_profile_pic = img_profile_pic.replace("_normal", "");
            }
            imageLoader.displayImage(img_profile_pic, imageView, options);
            holder.txtTweet_Id.setText("@" + object.getString("screenname"));
            //holder.txtTime.setText(object.getString("Id"));
            holder.txtFavCount.setText(Integer.toString(object.getInt("Fav_Count")));
            holder.txtRetweetCount.setText(Integer.toString(object.getInt("RT_Count")));
            getTimeDifference(object.getString("created_at"), holder.txtTime);
            Linkify.TransformFilter filter = new Linkify.TransformFilter() {
                public final String transformUrl(final Matcher match, String url) {
                    return match.group();
                }
            };

            Pattern mentionPattern = Pattern.compile("@([A-Za-z0-9_-]+)");
            String mentionScheme = "http://www.twitter.com/";
            Linkify.addLinks(holder.txtDesc, mentionPattern, mentionScheme, null, filter);

            Pattern hashtagPattern = Pattern.compile("#([A-Za-z0-9_-]+)");
            String hashtagScheme = "http://www.twitter.com/search/";
            Linkify.addLinks(holder.txtDesc, hashtagPattern, hashtagScheme, null, filter);

            Pattern urlPattern = Patterns.WEB_URL;
            Linkify.addLinks(holder.txtDesc, urlPattern, null, null, filter);
        } catch (JSONException e) {
            e.printStackTrace();
        }
        return convertView;
    }

    private void goToLoginScreen() {
        //pref = context.getSharedPreferences("");
        twitter = new TwitterFactory().getInstance();
        twitter.setOAuthConsumer("lpdiWZCCPkC5CSwdZJ91xLsrf","8cdgZjEfw8snsEqPV3ntPkYn5Klw432kroJCQFxEzqYJOo2bPY");
        new TokenGet().execute();
    }

    private void getTimeDifference(String pDate, TextView time) {
        int diffInDays = 0;
        SimpleDateFormat format = new SimpleDateFormat("yyyy-MM-dd'T'HH:mm:ss.SSS'Z'");
        format.setTimeZone(TimeZone.getTimeZone("GMT"));
        Calendar c = Calendar.getInstance();
        String formattedDate = format.format(c.getTime());

        Date d1 = null;
        Date d2 = null;
        try {

            try {
                d1 = format.parse(formattedDate);
                format.setLenient(true);
                d2 = format.parse(pDate);
            } catch (java.text.ParseException e) {
                e.printStackTrace();
            }
            long diff = d1.getTime() - d2.getTime();

            diffInDays = (int) (diff / (1000 * 60 * 60 * 24));
            if (diffInDays > 0) {
                if (diffInDays == 1) {
                    time.setText(diffInDays + "day ago");
                } else {
                    time.setText(diffInDays + " days ago");
                }
            } else {
                int diffHours = (int) (diff / (60 * 60 * 1000));
                if (diffHours > 0) {
                    if (diffHours == 1) {
                        time.setText(diffHours + " hr ago");
                    } else {
                        time.setText(diffHours + " hrs ago");
                    }
                } else {

                    int diffMinutes = (int) ((diff / (60 * 1000) % 60));
                    if (diffMinutes <= 2) {
                        time.setText("Just now");
                    } else {
                        time.setText(diffMinutes + " mins ago");
                    }

                }
            }
        } catch (ParseException e) {
            // System.out.println("Err: " + e);
            e.printStackTrace();
        }

    }
    private class TokenGet extends AsyncTask<String, String, String> {

        @Override
        protected String doInBackground(String... args) {

            try {
                requestToken = twitter.getOAuthRequestToken();
                oauth_url = requestToken.getAuthorizationURL();
            } catch (TwitterException e) {
                // TODO Auto-generated catch block
                e.printStackTrace();
            }
            return oauth_url;
        }

        @Override
        protected void onPostExecute(String oauth_url) {
            if(oauth_url != null){
                Log.e("URL", oauth_url);
                auth_dialog = new Dialog(context);
                auth_dialog.requestWindowFeature(Window.FEATURE_NO_TITLE);

                auth_dialog.setContentView(R.layout.twitter_auth);
                web = (WebView)auth_dialog.findViewById(R.id.webview1);
                web.getSettings().setJavaScriptEnabled(true);
                web.loadUrl(oauth_url);
                web.setWebViewClient(new WebViewClient() {
                    boolean authComplete = false;
                    @Override
                    public void onPageStarted(WebView view, String url, Bitmap favicon){
                        super.onPageStarted(view, url, favicon);
                    }

                    @Override
                    public void onPageFinished(WebView view, String url) {
                        super.onPageFinished(view, url);
                        if (url.contains("oauth_verifier") && authComplete == false){
                            authComplete = true;
                            Log.e("Url",url);
                            Uri uri = Uri.parse(url);
                            oauth_verifier = uri.getQueryParameter("oauth_verifier");

                            auth_dialog.dismiss();
                            new AccessTokenGet().execute();
                        }else if(url.contains("denied")){
                            auth_dialog.dismiss();
                            Toast.makeText(context, "Sorry !, Permission Denied", Toast.LENGTH_SHORT).show();


                        }
                    }
                });
                auth_dialog.show();
                auth_dialog.setCancelable(true);



            }else{

                Toast.makeText(context, "Sorry !, Network Error or Invalid Credentials", Toast.LENGTH_SHORT).show();
            }
        }
    }

    private class AccessTokenGet extends AsyncTask<String, String, Boolean> {


        @Override
        protected void onPreExecute() {
            super.onPreExecute();
            progress = new ProgressDialog(context);
            progress.setMessage("Fetching Data ...");
            progress.setProgressStyle(ProgressDialog.STYLE_SPINNER);
            progress.setIndeterminate(true);
            progress.show();

        }


        @Override
        protected Boolean doInBackground(String... args) {

            try {


                accessToken = twitter.getOAuthAccessToken(requestToken, oauth_verifier);
                SharedPreferences.Editor edit = pref.edit();
                edit.putString("ACCESS_TOKEN", accessToken.getToken());
                edit.putString("ACCESS_TOKEN_SECRET", accessToken.getTokenSecret());
                User user = twitter.showUser(accessToken.getUserId());
                profile_url = user.getOriginalProfileImageURL();
                edit.putString("NAME", user.getName());
                edit.putString("IMAGE_URL", user.getOriginalProfileImageURL());

                edit.commit();


            } catch (TwitterException e) {
                // TODO Auto-generated catch block
                e.printStackTrace();


            }

            return true;
        }
        @Override
        protected void onPostExecute(Boolean response) {
            if(response){
                progress.hide();
//                Fragment profile = new ProfileFragment();
//                FragmentTransaction ft = getActivity().getFragmentManager().beginTransaction();
//                ft.replace(R.id.content_frame, profile);
//                ft.setTransition(FragmentTransaction.TRANSIT_FRAGMENT_OPEN);
//                ft.addToBackStack(null);
//                ft.commit();

            }
        }


    }

    private class ViewHolder {
        public ImageView imgPic;
        public TextView txtTweet;
        public TextView txtDesc;
        public TextView txtTweet_Id;
        public TextView txtTime;
        public TextView txtRetweetCount;
        public TextView txtFavCount;
        public ImageView imgShare;
        public ImageView imgRetweet;
        public ImageView imgFav;
    }
}