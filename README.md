# visualizing a beer tent evacuation simulation

As task in the study project [Projektstudium Modellierungsseminar](http://fi.cs.hm.edu/fi/rest/public/modul/title/projektstudiummodellierungsseminar) with [Prof. Köster](http://www.cs.hm.edu/die_fakultaet/ansprechpartner/professoren/koester/index.de.html) in the WS2015/16 at the [University of Applied Sciences Munich](http://hm.edu/), we (8 students) made output of a 2D pedestrian simulation viewable in 3D in a [Google Cardboard](https://www.google.com/get/cardboard/). The scenario is a big beer tent during evacuation. The input files are coming from the 2D scenario we are modelling using [VADERE](http://www.multikosi.de/teilvorhaben-der-hm), a pedestrian simulator that the research group around Prof. Köster is developing.

We are building on the work of [Daniel Büchele](https://github.com/danielbuechele) and the additions made by [Florian Sesser](https://github.com/hacklschorsch) from [accu:rate](http://www.accu-rate.de/) regarding parsing files and a first version for Google Cardboard.

Three `apk`'s (*AgentView*, *CameraTour* and *FreeWalk*) from our final presentation can be downloaded [here](https://github.com/benjaminaaron/SumoVizUnity/releases/tag/Sprint3Release).
The 360° video as shown in the final presentation can be seen [here](https://youtu.be/5UxGIsptL5g). The unity asset *[360 Panorama Capture](https://www.assetstore.unity3d.com/en/#!/content/38755)* was used to create the 360° snapshots frame by frame.

![Screenshot](https://raw.githubusercontent.com/benjaminaaron/SumoVizUnity/master/BeerTentCardboardScreenshot.jpg)

This repository can be opened in the latest version of [unity](https://unity3d.com/). Run it by clicking the play button. To deploy it on a mobile phone use *File > Build & Run*.
