# VSLHOL03 Hands-On Lab: Legacy Lift-off

## Bringing Your Legacy .NET Application into the Modern Cloud and AI Age

### Hotel Management System demo application

This application is intended to demonstrate how to modernize a legacy Windows Forms application into an ASP.NET Core Blazor application with the aid of Generative AI. It was written in Windows Forms in .NET Framework 4.8 as a classic Client/Server application. Here are some screenshot:

![login](./code/start/legacy/Project_HMS/SnapShots/Screenshot%20(2054).png)

![rooms](./code/start/legacy/Project_HMS/SnapShots/Screenshot%20(2056).png)

### Hands on labs

In the hands on labs, we will bring this application into the modern age of AI and cloud by migrating and refactoring it. Meanwhile, we leverage the legacy code base as much as possible without doing a full rewrite of the application.

You will start with the code base in the [code/start/legacy folder](./code/start//legacy/) of this repo and work towards a more modern application. In case you get stuck, or want to continue from a certain point, you can refer to the sample solutions under [the code/final](./code/final) folder, which has a sample solution in several stages of completion.

You can clone this repo and start working on the labs which can be found on the [Wiki pages of this repository](https://github.com/XpiritCommunityEvents/LegacyLiftOffWorkshop/wiki). We recommend that you keep a tab open with the Wiki at all times during the lab.

Have fun!

### Cleanup

To clean up all the attendee repos:

```bash
for repo in $(gh repo list XpiritCommunityEvents --json name --jq '.[].name | select(test("^attendeello"))'); do
    echo "Deleting repository: $repo"
    gh repo delete "XpiritCommunityEvents/$repo" --yes
done
```
