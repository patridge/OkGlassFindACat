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
    public class RandomKittyActivity2 : Activity/*, GestureDetector.IOnGestureListener*/ {
        const string TAG = "RandomKittyActivity";
//        const string LIVE_CARD_ID = "RandomKitty";
        const string kittyJpgUrl = "http://thecatapi.com/api/images/get?format=src&type=jpg";

        ImageView kittyImageView;
//        GestureDetector gestureDetector;
//        TimelineManager timelineManager;
//        LiveCard liveCard;

        protected override void OnCreate(Bundle bundle) {
            base.OnCreate(bundle);
            SetContentView(Resource.Layout.card_kitty);
            kittyImageView = (ImageView)FindViewById(Resource.Id.kittyImage);

//            Intent menuIntent = new Intent(this, typeof(MenuActivity));
//            gestureDetector = new GestureDetector(this);
//            timelineManager = TimelineManager.From(this);
        }

        static async Task<Bitmap> GetRandomKitty() {
            var httpClient = new HttpClient(new OkHttpNetworkHandler());
            var imageBytes = await httpClient.GetByteArrayAsync(kittyJpgUrl);
            Bitmap imageBitmap = await BitmapFactory.DecodeByteArrayAsync(imageBytes, 0, imageBytes.Length);
            return imageBitmap;
        }
        async Task PresentNewRandomKitty() {
            try {
                var kittyBitmap = await GetRandomKitty();
                if (kittyBitmap != null) {
                    kittyImageView.SetImageBitmap(kittyBitmap);
                }
            }
            catch {
                kittyImageView.SetImageResource(Resource.Drawable.fallbackkitty1);
            }
            kittyImageView.Visibility = ViewStates.Visible;
        }
        protected async override void OnResume() {
            base.OnResume();

//            if (liveCard == null) {
//                Log.Debug(TAG, "Publishing LiveCard");
//                liveCard = timelineManager.GetLiveCard(LIVE_CARD_ID);
//
//                // Keep track of the callback to remove it before unpublishing.
//                callback = new ChronometerDrawer(this);
//                liveCard.EnableDirectRendering (true).SurfaceHolder.AddCallback (callback);
//                liveCard.SetNonSilent(true);
//
//                Intent menuIntent = new Intent(this, typeof(MenuActivity));
//                liveCard.SetAction(PendingIntent.GetActivity(this, 0, menuIntent, 0));
//
//                liveCard.Publish();
//                Log.Debug(TAG, "Done publishing LiveCard");
//            } else {
//                // TODO(alainv): Jump to the LiveCard when API is available.
//            }
            if (kittyImageView.Visibility != ViewStates.Visible) {
                await PresentNewRandomKitty();
            }
        }

        public override bool OnCreateOptionsMenu(IMenu menu) {
            Log.Debug(TAG, "OnCreateOptionsMenu");
            MenuInflater.Inflate(Resource.Menu.kitty, menu);
            return true;
        }
        public override bool OnOptionsItemSelected(IMenuItem item) {
            Log.Debug(TAG, "OnOptionsItemSelected");
            // Handle item selection.
            switch (item.ItemId) {
                case Resource.Id.finish:
                    Log.Debug(TAG, "OnOptionsItemSelected:finish");
                    Finish();
                    return true;
                case Resource.Id.another:
                    Log.Debug(TAG, "OnOptionsItemSelected:finish");
                    // Cheap way of doing async/await.
                    RunOnUiThread(async () => {
                        await PresentNewRandomKitty();
                    });
                    return true;
                default:
                    Log.Debug(TAG, "OnOptionsItemSelected:other");
                    return base.OnOptionsItemSelected(item);
            }
        }
        public override void OnOptionsMenuClosed(IMenu menu) {
            Log.Debug(TAG, "OnOptionsMenuClosed");
        }

//        public override bool OnGenericMotionEvent(MotionEvent e) {
//            gestureDetector.OnTouchEvent(e);
//            return false;
//        }
//        public bool OnDown(MotionEvent e) {
//            Log.Debug(TAG, "OnDown");
//            //Finish();
//            return false;
//        }
//        public bool OnFling(MotionEvent e1, MotionEvent e2, float velocityX, float velocityY) {
//            Log.Debug(TAG, "OnFling");
//            return false;
//        }
//        public void OnLongPress(MotionEvent e) {
//            Log.Debug(TAG, "OnLongPress");
//        }
//        public bool OnScroll(MotionEvent e1, MotionEvent e2, float distanceX, float distanceY) {
//            Log.Debug(TAG, "OnScroll");
//            return false;
//        }
//        public void OnShowPress(MotionEvent e) {
//            Log.Debug(TAG, "OnShowPress");
//        }
//        public bool OnSingleTapUp(MotionEvent e) {
//            Log.Debug(TAG, "OnSingleTapUp");
//
//            OpenOptionsMenu();
//            return false;
//        }
    }
}