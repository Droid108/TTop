package com.krishnak.tweetrap;

import android.app.Application;

import org.acra.annotation.ReportsCrashes;

/**
 * Created by SupportPedia on 21-08-2015.
 */
@ReportsCrashes(
        formKey = "", // This is required for backward compatibility but not used
        formUri = "https://collector.tracepot.com/2e0e3ce5"
)
public class TRApplication extends Application {
}
