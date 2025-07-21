============================================================================================================

# 1. Authentication & Identity (Azure AD B2C) multi-tenancy

1. Create an Azure AD B2C Tenant

- In Azure Portal, create a new Azure AD B2C tenant and link it to your subscription.

2. Register Applications (Frontend + Backend)

- Register your Angular frontend as a public client (SPA)
- Register your .NET backend as a web API
- Configure redirect URIs and expose scopes

3. Enable User Flows

- Set up sign-up and sign-in flows (email + password or social accounts like Google)

4. Integrate Angular with B2C

- Use MSAL.js (Microsoft Authentication Library)
- Prompt user login and acquire access token

5. Protect .NET API with B2C

- Validate JWT tokens in your ASP.NET Core API
- Add Authorize attributes on controllers

6. Detect Tenant from User Info

- Use the user's email domain or custom claim (e.g., extension_tenantId)
- Store and retrieve tenantId from your database

# 2. API Management (Azure API Management)

1. Create an API Management Instance

- In Azure, create a new API Management resource (Developer tier for testing)

2. Import Your API

- Import your existing Swagger/OpenAPI from the .NET backend

3. Create Products (Plans)

- Define plans like “Free”, “Pro”, and “Enterprise”
- Configure throttling and rate limits (e.g., 1000 calls/day for Free)

4. Assign Tenants to Products

- Generate subscription keys for each tenant
- Store the API key in your tenant database or user profile

5. Apply Policies

- Add policies to check usage, attach tenant ID, and restrict access

6. Optional: Setup Developer Portal

- Let tenant devs view API docs and request access

# 3. Email & Notification (SendGrid or Azure Communication Services)

1. Create SendGrid Resource or Azure Communication Services

- In Azure Marketplace, set up SendGrid and get API key

2. Set Up Email Templates

- Create reusable email templates (reminder, task assigned, etc.)
- Optionally brand by tenant (e.g., include tenant logo)

3. Build Notification Logic in .NET

- When a task is created with a due date, schedule a reminder

4. Use Azure Function for Scheduled Jobs

- Write a Timer Trigger Function (e.g., every 15 mins)
- Query tasks due in the next hour
- Send email via SendGrid

5. Track Delivery (Optional)

- Store sent emails, success/failure status per tenant

# 4. Monitoring & Logging (Application Insights)

1. Add Application Insights to Your App

- Add the SDK to your ASP.NET Core API
- Set the instrumentation key in config

2. Enrich Logs with Tenant Context

- Implement a TelemetryInitializer to attach tenantId to every telemetry item (requests, exceptions, dependencies)

3. Log User Actions

- Use ILogger to log user activities like task creation, updates, and errors

4. Create Custom Dashboard

- In Application Insights or Azure Monitor, build a dashboard that filters by tenantId

5. Set Up Alerts

- Configure alerts for anomalies or exceptions grouped by tenant

# Summary of Flow

1. Users sign up via Azure AD B2C
2. Users access APIs through APIM with rate limits
3. Scheduled reminders are emailed using Azure Function + SendGrid
4. Admins debug issues using tenant-based telemetry and logs in App Insights

============================================================================================================

# Case 1: user sign in with Microsoft Entra External ID
