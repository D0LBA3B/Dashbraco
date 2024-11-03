
# Dashbraco

**Dashbraco** is a comprehensive dashboard package for Umbraco, designed to provide site administrators with in-depth analytics and insights directly within the CMS. This project is actively developed to bring powerful features for optimizing content and enhancing user engagement.

## Current Features

- **Unused Media List**: Identify unused media items to optimize storage space and improve site performance.
  - **Media Reports**: Run reports to view all unused media and download results as a CSV file.
  - **Media Management**: Delete or move unused media to the recycle bin directly from the dashboard.
- **Widget Customization**: Enable or disable dashboard widgets to meet the specific needs of your site.
- **Picture of the Day**: Display an inspiring picture or video of the day from NASA’s APOD (Astronomy Picture of the Day) API.

## Planned Features

- **Real-Time Analytics**: View site data in real time, including visitors, page views, and average session duration.
- **Traffic Sources**: Understand where visitors are coming from and how they interact with content.
- **Page Performance**: Analyze loading times and user interactions for each page.
- **Advanced Dashboard Customization**: Create personalized dashboards with selected widgets and metrics.
- **Historical Data**: Access past statistics to identify trends and patterns.
- **Multi-Format Data Export**: Export reports in various formats, such as PDF, Excel, and more.
- **Automatic Alerts**: Receive alerts based on specific thresholds (e.g., sudden traffic spikes, page errors, etc.).
- **Advanced Google Analytics Integration**: Gain detailed user behavior insights and traffic sources through customized Google Analytics reports.

## Installation

The package will be available on Nuget

## Usage

Once installed, Dashbraco can be accessed in the "Dashboard" section of the Umbraco Backoffice. Available features include:

1. **Analytics Overview**: Displays a summary of real-time analytics, including visits and page views.
2. **Unused Media Management**: Run reports, export lists, and move unused media to the recycle bin for optimized management.
3. **Widget Customization**: Select the widgets most relevant to your dashboard.

> **Note**: Additional features are in development and will be added in future releases.

## Configuration

Dashbraco supports configuration settings in `appsettings.json`:

```json
{
    "Dashbraco": {
        "DefaultWidgets": ["Analytics", "PictureOfTheDay", "UnusedMedia"],
        "RefreshInterval": 300,
        "GoogleAnalyticsPropertyId": "PROPERTY_ID",
        "CredentialsPath": "credentials.json"
    }
}
```

- **DefaultWidgets**: Specifies which widgets are displayed on the dashboard by default.
- **RefreshInterval**: Sets how often data refreshes in seconds (default is 5 minutes).
- **GoogleAnalyticsPropertyId**: The property ID for Google Analytics integration.
- **CredentialsPath**: The path to the Google service account credentials file.

## Contribution

Contributions are welcome! Feel free to open issues or submit pull requests to enhance Dashbraco’s functionality.

## License

This project is licensed under the [GNU General Public License v3.0](LICENSE).

## Support

For any issues or feature requests, please open a [GitHub Issue](https://github.com/D0LBA3B/Dashbraco/issues).

---

Explore enhanced insights and analytics with **Dashbraco**, the ultimate dashboard package for Umbraco!
