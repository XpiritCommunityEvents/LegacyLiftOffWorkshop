# Fixing potential security issues in our codebase
The code that we have created at the moment is vulnerable and can be misused by malicious users. For this we need to find those vulerabilities and then fix them. In this lab we are going to use GitHub advanced Security to help us find known vulnerabilites in our code and we are going to use Copilot to help us resolve these issues.

## Discovering known vulnerabilities
We could of course go through the code line by line ourseves, and then see if we can find code that is vulnerable. This is in some cases very obvious, e.g. sql injection problems where we concatenate sql statements before we execute them and without checking the input we got from the user.

To help us find also the less obvious security vulnerabilities, we can use GitHub advanced security to help us with this.
This is done using gitHub actions and results in a nice list of items found, with detailed descriptions on what the issue might be. Lets start with enabling this in our repository.

GitHub advanced security is comprised out of three parts.
- Vulnerability scanning of dependencies
- Code Scanning of own code on known vulnerabilities
- Secret scanning, ensuring you don't add secrets to your source repository

The next three labs will help you enable this for our project.
 - [Lab-3.1-‐-Enabling-and-using-Dependabot-on-your-repository](Lab-3.1-‐-Enabling-and-using-Dependabot-on-your-repository.md)
- [Lab-3.2-‐-Enabling-Code-Scanning](Lab-3.2-‐-Enabling-Code-Scanning.md)
- [Lab-3.3-‐-Enable-Secret-Scanning](Lab-3.3-‐-Enable-Secret-Scanning.md)