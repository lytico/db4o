The files in this folder are repplacing <body> tag in the generated output to <body onload="prettyPrint()">.
This is required for code prettifier to work (i.e. add code coloring).
In order to use:
prettify.vbs [path_to_the_output]
For example:
prettify.vbs c:\Reference\Output\AllWeb\Content

Other components of the prettifier:
prettify.css - located in Resources\Stylesheets
prettify.js - located in Resources\Code (Used in MasterPage)
