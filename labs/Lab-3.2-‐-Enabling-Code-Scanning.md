## Objective

This hands-on lab has the goal to teach you how you can set up Code Scanning for your repository and to let you experience how its features help you to find security vulnerabilities and code errors in the code in your repository. Good luck! 👍

## Steps

### Step 1: :shield: Enable Code Scanning
Code scanning is a feature that you use to analyze the code in a GitHub repository to find security vulnerabilities and coding errors. Any problems identified by the analysis are shown in GitHub.

You can use code scanning to find, triage, and prioritize fixes for existing problems in your code. Code scanning also prevents developers from introducing new problems. You can schedule scans for specific days and times, or trigger scans when a specific event occurs in the repository, such as a push. If code scanning finds a potential vulnerability or error in your code, GitHub displays an alert in the repository. After you fix the code that triggered the alert, GitHub closes the alert. 

- Navigate to the `Settings` of your repository, click `Code Security & analysis` click `Set up | Default` for the `Code Scanning` feature.

![Code Scanning - Set up](./images/codescanningsetup.PNG)

- A screen pop ups where you can enable Code Scanning. 
- Keep the defaults and click the button <kbd>Enable CodeQL</kbd>
- After this step, a GitHub action is triggered in the background. 

### Step 2: :eyeglasses: View Code Scanning Results
Now we want to see the results of the various Code Scanning builds

- Navigate to the `actions` tab and click the CodeQL workflow.

From here, you can see the list of workflows with the CodeQL workflow and the run history of the CodeQL workflow. In the default CodeQL analysis workflow, code scanning is configured to analyze your code each time you either push a change to the default branch or any protected branches, or raise a pull request against the default branch. As a result, code scanning will now commence. As you can see on the below screenshot, the workflow is currently running.

### Step 3: :mag: Analyzing Code Scanning outcomes
- Navigate to the `Security` tab on your repository, and click `Code Scanning alerts`, you can see the active alerts for Code Scanning. From this view, you can view, fix, dismiss, or delete alerts for potential vulnerabilities or errors in your project's code.

- Each alert highlights a problem with the code and the name of the tool that identified it. You can see the line of code that triggered the alert, as well as properties of the alert, such as the severity, security severity, and the nature of the problem. Alerts also tell you when the issue was first introduced. For alerts identified by CodeQL analysis, you will also see information on how to fix the problem.

### Fixing SQL Injection issues
In the results you will find a couple of SQL injection vulnerabilities that have been found. You can use GitHub copilot to help you resule these issues quite easily. e.g. in the `RoomsController.cs` file there is an API call that can retrieve one room based on the id of the room. This id is passed as parameter in the query string and then it is used without any validation and concatenated to a SQL query. 
>**This is extreemly dangerous and an ideal way of an attacker to get all your data from the database and even delete data if they want !**

When you go to this file and select the implementation of the API call you can bring up the Copilot chat and give it the following prompt:
``` console
Can you rewrite this code so it is not succeptible anymore to a SQL injection attack
```

the result will be a suggestion to rewrite the code, including the explanation on how it changed the code. Accept the code and test the application, to see if it still works as expected. Now on the query string add the following, to see if it is still vulnerable: @@TODO FIND HACK On CURRENT IMPLEMENTATION.


## More Information
- [Managing code scanning alerts for your repository](https://docs.github.com/en/code-security/secure-coding/managing-code-scanning-alerts-for-your-repository]).

- [https://docs.github.com/en/code-security/code-scanning/automatically-scanning-your-code-for-vulnerabilities-and-errors/about-code-scanning](https://docs.github.com/en/code-security/code-scanning/automatically-scanning-your-code-for-vulnerabilities-and-errors/about-code-scanning)

- For more detailed information on managing the Code Scanning alerts, refer to: [https://docs.github.com/en/code-security/code-scanning/automatically-scanning-your-code-for-vulnerabilities-and-errors/managing-code-scanning-alerts-for-your-repository](https://docs.github.com/en/code-security/code-scanning/automatically-scanning-your-code-for-vulnerabilities-and-errors/managing-code-scanning-alerts-for-your-repository)

