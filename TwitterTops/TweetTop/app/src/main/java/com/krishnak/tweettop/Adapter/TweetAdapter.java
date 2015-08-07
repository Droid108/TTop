package com.krishnak.tweettop.Adapter;

import android.content.Context;
import android.text.util.Linkify;
import android.util.Patterns;
import android.view.LayoutInflater;
import android.view.View;
import android.view.ViewGroup;
import android.widget.BaseAdapter;
import android.widget.ImageView;
import android.widget.TextView;

import com.krishnak.tweettop.R;
import com.nostra13.universalimageloader.cache.memory.impl.WeakMemoryCache;
import com.nostra13.universalimageloader.core.DisplayImageOptions;
import com.nostra13.universalimageloader.core.ImageLoader;
import com.nostra13.universalimageloader.core.ImageLoaderConfiguration;
import com.nostra13.universalimageloader.core.assist.ImageScaleType;
import com.nostra13.universalimageloader.core.display.FadeInBitmapDisplayer;

import org.json.JSONException;
import org.json.JSONObject;

import java.util.ArrayList;
import java.util.regex.Matcher;
import java.util.regex.Pattern;

/**
 * Created by SupportPedia on 11-04-2015.
 */
public class TweetAdapter extends BaseAdapter {
    Context context;
    ArrayList<JSONObject> list;

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
            convertView.setTag(holder);
        } else {
            holder = (ViewHolder) convertView.getTag();
        }
        ImageLoader imageLoader = ImageLoader.getInstance();
        DisplayImageOptions options = new DisplayImageOptions.Builder().cacheInMemory(true)
                .cacheOnDisc(true).resetViewBeforeLoading(true).build();
        ImageView imageView = (ImageView) holder.imgPic;
        imageView.setImageResource(R.drawable.ic_launcher);
        JSONObject object = list.get(position);

        try {
            holder.txtTweet.setText(object.getString("name"));
            holder.txtDesc.setText(object.getString("text"));
            String img_profile_pic = object.getString("ProfileUrl");
            if(img_profile_pic.indexOf("normal") > 0)
            {
                img_profile_pic = img_profile_pic.replace("_normal","");
            }
            imageLoader.displayImage(img_profile_pic, imageView, options);
            holder.txtTweet_Id.setText("@" + object.getString("screenname"));
            holder.txtTime.setText(object.getString("Id"));
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

    private class ViewHolder {
        public ImageView imgPic;
        public TextView txtTweet;
        public TextView txtDesc;
        public TextView txtTweet_Id;
        public TextView txtTime;
    }
}