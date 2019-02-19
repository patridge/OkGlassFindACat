ok glass...find a cat
=====================

Taking a shot at making a voice-triggered immersive Google Glass app (Glassware, I guess) using Xamarin.Android.

## Background

Originally, I spun up a new Xamarin.Android project, added the [Glass component](https://components.xamarin.com/view/googleglass), and hit run. I was presented with a tile showing the button click counter sample. This is a hybrid of that with the Xamarin Glass sample (translated from the [Glass demos](https://github.com/googleglass)) designed to download and present a random cat picture.

![Some random cat from The Cat API](http://thecatapi.com/api/images/get?format=src&type=jpg)

## Debug/Installation Steps

1. Put Glass into debug mode and plug it in via USB cable.
2. Tell Xamarin Studio to run and point the target at the Glass.
3. Run the RandomKitty project.
    1. If Glass is active, it should just run and load a random cat picture from [the cat API](http://thecatapi.com/).
    2. If Glass wasn't active when you started debugging, you will need to trigger it via the voice command (see next section).

## Getting a Kitty

1. Go to the [Home card](https://developers.google.com/glass/design/ui/timeline#home)
2. Say "ok glass".
3. Then say "find a cat". (Originally, it was "get a kitty", but Glass doesn't seem to like how I pronounce kitty ["kid-ee"].)

## Next Steps

This is a first run at a Glass app, so I won't pretend I did anything properly. I will pretend that it seems to work. I mean, I did get to see random kitty pictures.

Obviously, there is much to do beyond here, both in incoporating Glass functionality for demonstration purposes and in making this app easier to use.

* Incorporate live tile or mirror tile usage. Obviously, you should be able to save the best cat pictures to your timeline for fond memories later.
* Glass-friendly gesture support. Simply implementing `GestureDetector.IOnGestureListener` doesn't seem to cut it. The `OnSingleTapUp` system seems to be insufficiently sensitive; you really have to tap with no movement to trigger the menu. The [GDK article on detecting gestures](https://developers.google.com/glass/develop/gdk/input/touch) mentions comparing a generic gesture to `gesture.TAP` (also `TWO_TAP`, `SWIPE_RIGHT`, and `SWIPE_LEFT`). I am guessing these gestures are based around a system with more tolerance for movement fluctuations.

## Recent Additions

* Loading progress. ProgressBar control worked just fine. It could look more Glass-like with some styling (read: thicker and white), but it is fine for now.
* {fixed} Memory management. Loading additional cat now attempts to free up references to old one.
