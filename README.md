# Contoso Insurance UWP App
This UWP app was designed to showcase how Fluent Design, Windows Timeline and Adaptive Cards work together on the Windows Developer Day 2018.

[View the full WDD Keynote here](https://www.youtube.com/watch?v=D6YAJxFsmuM)

Key UI components
------

`MainPage.xaml` is the top-level page that contains the background image, a `NavigationView` and a `NotificationDialog` control on the bottom right corner of the page.

* The `NavigationView` contains the main menu and a `ContentFrame` that basically hosts all other pages such as `HomePage`, `CasesPage` and `CasePage`.
    * `HomePage` contains the Telerik `RadCartesianChart` control and a few other dummy UI elements.
    * `CasesPage` contains an extended `AdaptiveGridView` control that displays a list of cases.
    * `CasePage` is the detail page of one particular case. This is where we create user activities and push the adatpive card (`timeline.json`) to Windows Timeline.
* `NotificationDialog` contains a Bing `MapControl`, a `ListView` that shows a list of adjusters, and some adaptive card UI that's generated from `bot1.json`, `bot2.json` and `bot3.json`. The use of `AcrylicBrush` is in this control too.

For demo purpose, you can enable the *adjuster view* by clicking/tapping on the avatar from the bottom left of the main page.

<sub>P.S. *Bahnschrift* is the font that's used throughout the project. :)</sub>
