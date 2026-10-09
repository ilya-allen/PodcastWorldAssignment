What has been done?

Created making episodes and viewing episodes from PODCASTERS POINT OF VIEW AND LISTENERS POV
Subscribing to podcasts
Saving Playback

For next time prioritise:
Not allowing nonregular listeners to listen to podcast that is premium user only
Add listening to subscribed podcasts



FINAL CLEANUP:
In menu view, their is a -99999, this is a code smell and I will need to fix it such that whenever a Non-Int number is passed, the correct protocol is done, instead of just trying to quickly bypass it for the test case. This can be done with podcast consts

There is another code smell in the PodcasterRegistration for choosing genre. I decided to do a quick fix of making a public read genre option and the entire genre option is pretty smelly in user service. This should be fixed before submission

Maybe adjust padding between text in all the displays where they need to look like tables!

I need to add a take an integer range. Too many times am i calling a public integer range. Need to fix it.

Ctrl F for // Code Smell