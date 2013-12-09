using Android.App;
using Android.Views;
using Android.OS;
using Android.Widget;
using Android.Graphics;
using System.Net.Http;
using ModernHttpClient;
using System;
using System.Threading.Tasks;
using Android.Util;
using Android.Glass.Timeline;

namespace KittyGlass {
    [Activity(Label = "@string/app_name", MainLauncher = true, Theme = "@style/MenuTheme", Enabled = true)]
    [IntentFilter(new[] { "com.google.android.glass.action.VOICE_TRIGGER" })]
    [MetaData("com.google.android.glass.VoiceTrigger", Resource = "@xml/voice_trigger_start")]
    public class RandomKittyActivity : Activity, GestureDetector.IOnGestureListener {
        const string TAG = "RandomKittyActivity";
        const string kittyJpgUrl = "http://thecatapi.com/api/images/get?format=src&type=jpg";

        ImageView kittyImageView;
        ProgressBar kittyProgressBar;
        GestureDetector gestureDetector;

        protected override void OnCreate(Bundle bundle) {
            base.OnCreate(bundle);
            SetContentView(Resource.Layout.card_kitty);
            kittyImageView = (ImageView)FindViewById(Resource.Id.kittyImage);
            kittyProgressBar = (ProgressBar)FindViewById(Resource.Id.kittyProgress);
            gestureDetector = new GestureDetector(this);
        }

        static async Task<Bitmap> GetRandomKitty() {
            var httpClient = new HttpClient(new OkHttpNetworkHandler());
            var imageBytes = await httpClient.GetByteArrayAsync(kittyJpgUrl);
            Bitmap imageBitmap = await BitmapFactory.DecodeByteArrayAsync(imageBytes, 0, imageBytes.Length);
            return imageBitmap;
        }
        async Task PresentNewRandomKitty() {
            try {
                kittyProgressBar.KeepScreenOn = true;
                kittyProgressBar.Visibility = ViewStates.Visible;
                var kittyBitmap = await GetRandomKitty();
                if (kittyBitmap != null) {
                    kittyImageView.SetImageBitmap(kittyBitmap);
                }
                kittyProgressBar.Visibility = ViewStates.Invisible;
                kittyProgressBar.KeepScreenOn = false;
            }
            catch {
                kittyImageView.SetImageResource(Resource.Drawable.fallbackkitty1);
            }
            kittyImageView.Visibility = ViewStates.Visible;
        }
        protected async override void OnResume() {
            base.OnResume();

            if (kittyImageView.Visibility != ViewStates.Visible) {
                await PresentNewRandomKitty();
            }
        }

        public override bool OnCreateOptionsMenu (IMenu menu) {
            MenuInflater.Inflate(Resource.Menu.kitty, menu);
            return true;
        }
        public override bool OnOptionsItemSelected (IMenuItem item) {
            // Handle item selection.
            switch (item.ItemId) {
                case Resource.Id.another:
                    PresentNewRandomKitty();
                    return true;
                case Resource.Id.finish:
                    Finish();
                    return true;
                default:
                    return base.OnOptionsItemSelected (item);
            }
        }

        // Until I can figure out how to tie an Activity to the gesture types mentioned on the GDK, this will do.
        public override bool OnGenericMotionEvent(MotionEvent e) {
            gestureDetector.OnTouchEvent(e);
            return false;
        }
        public bool OnDown(MotionEvent e) {
            //Log.Debug(TAG, "OnDown");
            // Glass seems to handle down swipe automatically, but it could be worth killing here, too.
            //Finish();
            return false;
        }
        public bool OnFling(MotionEvent e1, MotionEvent e2, float velocityX, float velocityY) {
            //Log.Debug(TAG, "OnFling");
            // Implement any in-app swiping use.
            return false;
        }
        public void OnLongPress(MotionEvent e) {
            //Log.Debug(TAG, "OnLongPress");
        }
        public bool OnScroll(MotionEvent e1, MotionEvent e2, float distanceX, float distanceY) {
            //Log.Debug(TAG, "OnScroll");
            return false;
        }
        public void OnShowPress(MotionEvent e) {
            //Log.Debug(TAG, "OnShowPress");
        }
        public bool OnSingleTapUp(MotionEvent e) {
            //Log.Debug(TAG, "OnSingleTapUp");
            // Show the menu. This seems to require a very explicit tap (with no side movement).
            // Here's hoping the GDK gestures build in some realistic tolerance values.
            OpenOptionsMenu();
            return false;
        }
    }
}