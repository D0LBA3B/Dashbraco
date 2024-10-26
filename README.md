
# Dashbraco

Dashbraco is a comprehensive dashboard package for Umbraco, designed to provide site administrators with detailed analytics and insights directly within the CMS. Easily track page views, visitor statistics, and performance metrics to optimize content and enhance user engagement.

## Features

- **Real-Time Analytics**: View real-time data on site visitors, page views, and session duration.
- **Page Performance**: Analyze each page's loading time and user interactions.
- **Traffic Sources**: Understand where your visitors are coming from and how they interact with your content.
- **Customizable Dashboards**: Create personalized dashboards with widgets and metrics that matter to you.
- **Historical Data**: Access past statistics to spot trends and patterns.

## Installation

### 1. Using NuGet

To install Umbralytics via NuGet, run the following command in your NuGet Package Manager Console:

```shell
Install-Package Dolba@Dashbraco
```

### 2. Manual Installation

1. Download the latest release from the [Dashbraco GitHub Repository](https://github.com/D0LBA3B/Dashbraco/releases).
2. Extract the downloaded package and copy the contents to your Umbraco project.
3. In Umbraco Backoffice, navigate to the settings panel to activate the Dashbraco dashboard.

## Usage

After installation, you'll find Dashbraco under the "Dashboard" section in your Umbraco Backoffice. Here you can:

1. **View Analytics Overview**: Displays a summary of real-time analytics.
2. **Customize Metrics**: Choose the metrics that matter most to you.
3. **Export Data**: Download reports for offline analysis.

## Configuration

Dashbraco offers several customizable settings:

- **Default Dashboard View**: Choose which widgets to display by default.
- **Refresh Interval**: Set how often the data refreshes (default is every 5 minutes).
- **User Permissions**: Restrict access to the dashboard based on user roles.

Add these configurations to your Umbraco `appsettings.json` if needed:

```json
{
    "Dashbraco": {
        "DefaultWidgets": ["Page Views", "Traffic Sources", "Loading Time"],
        "RefreshInterval": 300
    }
}
```

## Contribution

Contributions are welcome! Feel free to open issues or submit pull requests to enhance the functionality of Umbralytics.

## License

This project is licensed under the GNU General Public License v3.0 - see the [LICENSE](LICENSE) file for details.

## Support

For any issues or feature requests, please open a [GitHub Issue](https://github.com/D0LBA3B/Dashbraco/issues)

---

Enjoy enhanced insights and analytics with **Dashbraco**, the ultimate dashboard package for Umbraco!
