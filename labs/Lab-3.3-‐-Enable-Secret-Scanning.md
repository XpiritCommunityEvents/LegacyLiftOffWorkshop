## Objective

This hands-on lab has the goal to teach you how you can set up and  enable Secret Scanning and will experience how it is able to scan your repository for known types of secrets and what its capabilities are in order to prevent fraudulent use. Good luck! 👍

## Steps

### Step 1: :shield: Secret Scanning
If your project communicates with an external service, you might use a token or private key for authentication. Tokens and private keys are examples of secrets that a service provider can issue. If you check a secret into a repository, anyone who has read access to the repository can use the secret to access the external service with your privileges. We recommend that you store secrets in a dedicated, secure location outside of the repository for your project.

Secret scanning will scan your entire Git history on all branches present in your GitHub repository for any secrets. Service providers can partner with GitHub to provide their secret formats for scanning. For more information, see [Secret scanning partner program](https://docs.github.com/en/code-security/secret-scanning/secret-scanning-partner-program).

If someone checks a secret with a known pattern into a public or private repository on GitHub, secret scanning catches the secret as it's checked in, and helps you mitigate the impact of the leak. Repository administrators are notified about any that contains a secret, and they can quickly view all detected secrets in the Security tab for the repository.

- Navigate to the `Settings` of your repository, click `Code Security & analysis` click `Enable` for the `Secret Scanning` feature.

Secret Scanning is now enabled for your repository. 

GitHub performs secret scanning on public and private repositories for secret patterns provided by GitHub and GitHub partners. However, there can be situations where you want to scan for other secret patterns in your private repositories. For example, you might have a secret pattern that is internal to your organization. For these situations, you can define custom secret scanning patterns in your enterprise, organization, or private repository on GitHub.

You can define up to 20 custom patterns for each private repository, organization, or enterprise account and options to define these are enabled after you enable the `Secret Scanning` feature in your `Security & analysis` settings. We'll not go into the custom patterns in this lab, but for more information refer to: [https://docs.github.com/en/code-security/secret-scanning/defining-custom-patterns-for-secret-scanning](https://docs.github.com/en/code-security/secret-scanning/defining-custom-patterns-for-secret-scanning)

Now that you've set up Secret Scanning, let's understand where you can view the Secret Scanning alerts and ultimately try it out and trigger it ourselves by inserting a connection string in the next parts of the lab exercise.

### Step 2: :lock_with_ink_pen: Managing Secret Scanning alerts
- Navigate to the `Security` tab on your repository, and click `Secret Scanning alerts`, you can see the active alerts for Secret Scanning. Here, you can view and close alerts for secrets checked in to your repository.
- When you click on a Secret Scanning alert, you can see it's details.

### Step 3: :rotating_light: Secret Scanning: Alert notifications
When a new secret is detected, GitHub notifies all users with access to security alerts for the repository according to their notification preferences. You will receive alerts if you are watching the repository, have enabled notifications for security alerts or for all the activity on the repository, are the author of the commit that contains the secret and are not ignoring the repository.

Once a secret has been committed to a repository, you should consider the secret compromised. GitHub recommends the following actions for compromised secrets:

* For a compromised GitHub personal access token, delete the compromised token, create a new token, and update any services that use the old token. 
* For all other secrets, first verify that the secret committed to GitHub is valid. If so, create a new secret, update any services that use the old secret, and then delete the old secret.

### :bulb: Triggering Secret Scanning by inserting a connection string
You can trigger the Secret Scanning functionality by inserting a secret in your repository yourself. To do so, try if you can manage to execute the following steps:
* Go to the [Azure Portal](https://portal.azure.com/)
* Create, for example, a storage account within your own subscription
* Once this is created, copy the connection string to the storage account (navigate to `Access keys`)
* Go back to your code
* Open the file `<yourrepo>/frontend/appsettings.json`, 
* At the end of the file, append the file with a `StorageAccountConnectionString` element and paste the copied connection string. 
* Commit your changes and see whether you have been able to trigger a Secret Scanning alert (give the workflow some time to run after the commit) 

### :pilot: Using Copilot to generate a custom secret pattern 
What if you have some patterns within your company that you never want to commit to your source contol system. You can also use GHAS Secret scanning to add custom patterns. This is all based on Regular Expressions. So, let's as Copilot to generate a Regular Expression for a custom patterns.

- Open Copilot chat in your Codespace by pressing the chat balloons in your left hand command bar
- Ask Copilot to generate a regular expression for this demo data 
    - bcmp_aaht5564papqq-dsd9229s
    - bcmp_983wejxnczldd-dfihdsf3
    - bcmp_3489jdsblzdvh-ewrvc3dc
    - bcmp_bhb87wejkdvje-asdvne3r
    - bcmp_okdf9cm8i4kdd-sdfgd3ss
- Ask `Generate a RegEx to match this data` and paste this data
- In you repository, open Settings and navigate to Code Security & Analysis
- Scroll to the Secret settings and press `New Custom Pattern`
- Use the RegEx and save the custom pattern
- Take one of the examples above and try to check this in, by modifying a codefile


## More Information

- For GitHub Secret Scanning documentation, please refer to: [https://docs.github.com/en/code-security/secret-scanning/about-secret-scanning](https://docs.github.com/en/code-security/secret-scanning/about-secret-scanning)

- For more information, please refer to: [https://docs.github.com/en/code-security/secret-scanning/managing-alerts-from-secret-scanning](https://docs.github.com/en/code-security/secret-scanning/managing-alerts-from-secret-scanning)
