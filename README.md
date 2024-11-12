
# Dashbraco

**Dashbraco** is a comprehensive dashboard package for Umbraco, designed to provide site administrators with in-depth analytics and insights directly within the CMS. This project is actively developed to bring powerful features for optimizing content and enhancing user engagement.

## Current Features

- **Unused Media List**: Identify unused media items to optimize storage space and improve site performance.
  - **Media Reports**: Run reports to view all unused media and download results as a CSV file.
  - **Media Management**: Delete or move unused media to the recycle bin directly from the dashboard.

- **Recent User Activities**: Monitor and display the latest activities performed by users on your site.      
  - **Activity Feed**: View a detailed list of recent actions such as content publishing, editing, unpublishing, and more.
  - **User Avatars**: Display user avatars next to their activities for easy identification.
  - **Interactive Links**: Quickly navigate to the related content items linked to each activity.

- **Widget Customization**: Enable or disable dashboard widgets to meet the specific needs of your site.
- **Picture of the Day**: Display an inspiring picture or video of the day from NASA’s APOD (Astronomy Picture of the Day) API.
- **Analytics Overview**: View analytics data in real-time including the number of visitors, page views, sessions, and bounce rate, with graphical representation of active users.

## Planned Features

- **Traffic Sources**: Understand where visitors are coming from and how they interact with content.
- **Page Performance**: Analyze loading times and user interactions for each page.
- **Advanced Dashboard Customization**: Create personalized dashboards with selected widgets and metrics.
- **Historical Data**: Access past statistics to identify trends and patterns.
- **Multi-Format Data Export**: Export reports in various formats, such as PDF, Excel, and more.
- **Automatic Alerts**: Receive alerts based on specific thresholds (e.g., sudden traffic spikes, page errors, etc.).
- **Advanced Google Analytics Integration**: Gain detailed user behavior insights and traffic sources through customized Google Analytics reports.

## Installation
[![NuGet version (Our.Umbraco.Dashbraco)](https://img.shields.io/nuget/v/Our.Umbraco.Dashbraco.svg?style=flat-square)](https://www.nuget.org/packages/Our.Umbraco.Dashbraco/)

You can install Dashbraco via [NuGet](https://www.nuget.org/packages/Our.Umbraco.Dashbraco/):

```bash
dotnet add package Our.Umbraco.Dashbraco
```



## Demo
_All colors are editable & tabs can be disabled/enabled via appsettings (my choice of color is disgusting I guess ahaha)_

![image](https://github.com/user-attachments/assets/74573ebb-7e67-43ac-86c3-180a9ea81d1b)


![image](https://github.com/user-attachments/assets/556209bc-ef0c-4ead-9041-c91d13cf9070)


![image](https://github.com/user-attachments/assets/5839fd20-346e-4645-802e-09388d6c8f1f)



## Configuration

Dashbraco supports configuration settings in `appsettings.json`:

```json
  "Dashbraco": {
    "DefaultWidgets": [ "Analytics", "PictureOfTheDay", "UnusedMedia", "EntriesActivites" ],
    "RefreshInterval": 300,
    "GoogleAnalyticsPropertyId": "99999999",
    "GoogleCredentials": {
      "type": "service_account",
      "project_id": "YOUR_PROJECT_ID",
      "private_key_id": "YOUR_PRIVATE_KEY_ID",
      "private_key": "-----BEGIN PRIVATE KEY-----\nYOUR_PRIVATE_KEY\n-----END PRIVATE KEY-----\n",
      "client_email": "YOUR_CLIENT_EMAIL",
      "client_id": "YOUR_CLIENT_ID",
      "auth_uri": "https://accounts.google.com/o/oauth2/auth",
      "token_uri": "https://oauth2.googleapis.com/token",
      "auth_provider_x509_cert_url": "https://www.googleapis.com/oauth2/v1/certs",
      "client_x509_cert_url": "YOUR_CERT_URL"
    },
    "Styles": {
      "PrimaryColor": "#F3F3F3",
      "SecondaryColor": "#F3F3F3",
      "TextColor": "#333333",
      "ActiveTabColor": "#A8322E",
      "InactiveTabColor": "#FF5733",
      "BackgroundColor": "#FFFFFF",
      "BorderColor": "#E0E0E0",
      "HoverColor": "#FF7043",
      "ButtonColor": "#3BCF0A",
      "ButtonTextColor": "#FFFFFF",
      "SuccessColor": "#28A745",
      "WarningColor": "#FFC107",
      "ErrorColor": "#C0392B",
      "LinkColor": "#007BFF",
      "LinkHoverColor": "#0056B3"
    }
  },
```


## Contribution

Contributions are welcome! Feel free to open issues or submit pull requests to enhance Dashbraco’s functionality.

## License
[![License: GPL v3](https://img.shields.io/badge/License-GPLv3-blue.svg)](https://www.gnu.org/licenses/gpl-3.0)

This project is licensed under the [GNU General Public License v3.0](LICENSE).

## Support

For any issues or feature requests, please open a [GitHub Issue](https://github.com/D0LBA3B/Dashbraco/issues).

---

Explore enhanced insights and analytics with **Dashbraco**, the ultimate dashboard package for Umbraco!
