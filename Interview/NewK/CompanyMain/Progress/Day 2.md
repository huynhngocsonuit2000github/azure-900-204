# Day 2 Blazor (what interviewers expect)

- Blazor: is the way to build web UI using C# + Razor components instead of JavaScript frameworks
- Component: is the basic building block (like Angular/ React component)
  - File: Something.razor
  - Contains:
    - Markup (HTML-like)
    - C# code (state + event handlers)
  - For example: TodoList.razor to render list, TodoItem.razor to render one row
- Razor syntax:

  - Razor means we can mix HTML and C#
    - @someVariable
    - @if
    - @foreach

- Parameters (parent → child): using parameters
  // [Parameter] public Todo Item {get;set;}
  // <TodoItem Item="t" />

- Events (child → parent): using EventCallback
  // EventCallback<Todo> OnDelete
  // <TodoItem OnDelete="HandleDelete" />

- State (why UI updates): C# field/property
  // Blazor re-render = update UI when state changes.

- Rendering (what happens under the hood)
  - Blazor Server: UI logic runs on server; browser sends events via SignalR.
  - Blazor WebAssembly (WASM): UI logic runs in the browser.
- Routing (pages)
  - Component becomes a page using:
    // @page "/todos"
  - Then navigation:
    // <NavLink href="/todos">Todos</NavLink>
- Layout

  - share the same menu/header/footer
    // MainLayout.razor wrap pages using @Body

- DI services (how you call APIs cleanly)

```cs
// Program.cs
builder.Services.AddScoped<TodoApiClient>();

// TodoApiClient.cs
public sealed class TodoApiClient
{
    private readonly HttpClient _http;

    // Call API from microservice to get data
    public Task<List<TodoDto>?> GetTodosAsync()...
}

// Todos.razor
@page "/todos"                  // url
@inject TodoApiClient Api       // inject DI service

// Render page
<ul>
    @foreach (var t in _todos) { <li>@t.Title</li> }
</ul>

// Code c# inside the razor page
@code {
  private List<TodoDto>? _todos;

  protected override async Task OnInitializedAsync()
    => _todos = await Api.GetTodosAsync();
}
```

- HttpClient (calling microservice APIs)
  - Bad: using HttpClient to call APIs directly
  - Good: create new DI service, inside that using HttpClient to call API and expose the method for razor page to call that method
- Forms + validation
- Auth (only the core idea)

## Server vs WASM trade-offs, rendering, state management

### Question

- What is Blazor WebAssembly?
  - Run .NET UI code in the browser (WASM). Network is mainly for API calls, not UI event
- Blazor WASM vs Blazor Server — main trade-offs?
  - WASM: heavier first load, fast interaction after
  - Server: fast first load, but latency affects interaction, the server will hold per-user state
- “How do you handle auth for WASM calling APIs?” (tokens, refresh, storage risks, prefer BFF)
- What causes slow first load in WASM and how do you improve it?
  - Browser need to download runtime/assemblies all the necessary material for the first time. We can improve compression, trimming, lazy loading, reduce dependencies
  - [Lazy-loading]:
    - User click to app -> browser just download the base WASM + core assemblies.
    - User navigates to another tab (/admin)
    - App downloads only that feature's assemblies
    - Then it renders the feature page

```cs
// AdminRoute.razor (a tiny “gate” page)
@page "/admin"
@inject LazyAssemblyLoader Loader
@inject NavigationManager Nav

<p>Loading admin...</p>

@code {
  protected override async Task OnInitializedAsync()
  {
    await Loader.LoadAssembliesAsync(new[] { "MyApp.AdminFeature.dll" });
    Nav.NavigateTo("/admin/home", forceLoad: false);
  }
}
```

- What is trimming? Risks?
  - Publish removes unused code to reduce size. Risk: breaks reflection/dynamic usage -> need to testing and sometimes linker config
- What is AOT in WASM? Why trade-off?
  - [Ahead-of-time] compile improve runtime speed but increase download size and build time
- How do you call microservice APIs from Blazor WASM?
  - Use HttpClient, but often create another ApiClient service to call to BFF/gateway rather than many service directly
- How do you handle [auth] in [WASM] when [calling-APIs]?
  - Actually, we can call the API directly from browser, just need to get the access token from the storage/refresh/CORS
  - But in my project BFF: browser use HttpOnly cookies to BFF, BFF will extract the token from the cookies and attach to the authorize header to call microservice API
- Why is storing tokens in localStorage risky?
  - XSS can steal tokens, prefer to use BFF + HttpOnly cookie
  - Because if the app has an XSS issue, attacker can run JS in the page directly
- What is a BFF and why use it with WASM?
  - Backend dedicated for frontend: handles OIDC(OAuth 2), keeps tokens server-side, avoid CORS
- How do you centralize API error handling (401/500) in WASM?
  - Option 1 (easy + common): wrap HttpClient in one service, will redirect the unauthorized user to the login page
- How do you implement timeouts and retries for WASM API calls?
  - Set client timeouts, apply per-endpoints, also need set the limit max number of retries. It is to ensure the caller stop retrying many time to the failed server
- What about CORS in WASM?
  - Only needed when the browser calls a different origin. BFF same-origin reduces CORS complexity
- How do you manage state in Blazor WASM?
  - Component state for local UI (c# variable on the razor page); scoped state services for shared state (create the Service class and register with DI, and inject that service into the component); persist only what needed (and safely).
- How do you handle large data lists (feeds) efficiently?
  - Pagination (just render the necessary item on the UI)/infinite scroll, virtualization, caching, avoid re-rendering huge DOM (Using @key to avoid re-render).
- “How do you avoid calling 5 microservices from the UI for one screen?” (BFF aggregation / gateway / caching)
  - BFF aggregation:
    - Combine 5 API request into only one API call, for example /bff/home
    - BFF can shape data exactly for that screen
  - Caching
    - To avoid calling to Microservice API many time
    - Share the caching for the public data
    - Cache the data on the client site
      -> I do not let the UI call many microservice API if within the same page, I create one BFF endpoint per screen (like /bff/home) on that BFF API I will call to multiple microservice APIs parallel and return one DTO. And I also cache the response to avoid that case, which user refresh many times, just set the short timeout about 10 second
- How do you debug performance issues in WASM?
  - Case Startup / download is slow:
    - check the total MB \_framework downloaded
    - remove the heavy packages, enable trimming, lazy-load feature
  - UI feels laggy
    - check the long tasks and frequent re-render patterns
    - use <Virtualize> for big lists
    - avoid "set state" in loops
    - add @key in lists
    - split the large components into smaller ones
  - Too many API calls
- How do you handle transient failures?” (timeouts, retries, circuit breaker, idempotency)
  - Transient failures: this is the temporary problem: network issue, service overload, timeout just face the issue for a short time
  - With each case we will handle different way
    - Set timeout for each API call to the downstream service, if it is failed return the error
    - Apply retry, but need to apply careful, because it can lead to duplicated data issue
    - Stop calling to the broken service, if we call to that service many time but still failed
      -> If we do not handle the transient failure, it can impact to the user experient, faile to load screen, request hang (if we not apply timeout - the thread connection can be not available) lead to the service slow down or crashes. Specially on the Microservice, it can lead to Cascading failure, because if one service at the downstream system faced slow performance, it can impact to all the upstream service, and everything looks down.
- CORS: where and why? (only when browser calls APIs directly)
  - When the UI call the the API with different domain. For example, the Blazor WASM call the microservice APIs
- Where to store token (avoid unsafe storage if possible)
  - The HttpOnly cookie is an authentication session for the BFF, when the broser send the cookies, the BFF will know how the user is
- Refresh token flow
  - UI call BFF to get data
  - BFF extract access/refresh token from auth cookies
  - If the acess token is invalid, BFF will use that refresh token to call the Azure Entra to get the new access token
  - BFF store the new access token to the server cache and use that to call microservice API to get data and return the UI
- Token expiry handling (retry once after refresh)

## Auth integration basics, API calling patterns

- OAuth2: Azure Entra Authentication
- Where we can store tokens:
  - localStorage: XSS risk
  - memory: lost on refresh
  - BFF: HttpOnly secure cookies
- API calling pattern
  - Direct WASM -> API: WASM store the token on the UI site in the localStorage(XSS risk), local memory (lost on refresh) and attach the access token into the Authorization token
  - WASM -> BFF -> API: call /bff/api/\* same origin, BFF read the Auth cookies to get the access token and attach to the api calling to microservice API
  - Downstream APIs: use on [todo]
- Basic flow
  - Login: WASM -> BFF /login -> Entra (IdP) -> cookies
  - Call API: WASM -> BFF -> API (attach the token)
  - Refresh: BFF will handle getting new access token by refresh token behind the scenes
  - WASM

### Question

- Why localStorage is dangerous?
  - Accessible by Javascript
  - the token can be stolen
    -> To fix that, we can use HttpOnly + Secure cookies (BFF). Or we can apply the short-lived token to avoid the token is stolen
- How to call multiple APIs securely? [todo]
  - Set time out for each API calling
  - Apply retry and set the limit number of retry failed
- How logout works in SPA?

  - SPA logon within BFF
    - WASP call /logon
    - BFF: clear cookies, remove access token and refresh token in the cache, redirect to IdP logout
    - Browser redirect back

- Explain your auth flow end-to-end

  - UI → BFF login cookie → BFF gets token → Gateway validates → Gateway check the DB permission check → microservice

- Why BFF for WASM instead of storing tokens in browser?
  - Storing Token in the server side can be stolen, lead to XSS risk, refresh is safer, with BFF we can cache the token at the server side, and set authenticated for the UI by using HttpOnly Auth Cookies
- Cookie session vs access token?
  - Cookie = session to BFF (browser ↔ BFF).
  - Access token = proof to call API/Gateway (BFF ↔ Gateway).
- Where do you perform authorization and why?
  - Gateway perform the real authorization (single point), in my project, I create the custom authorization, in which I read the user id from the JWT access token, and query in the database to check if that user is valid to access to the microservice API
- Avoid DB permission lookup every request?
  - Cache the permission, and auto refresh after 5-10 minutes for example
- How to pass user identity to microservices safely
  - Call by Private network
  - Only gateway can call micro service
  - After the request pass through gateway, we will adds X-User-Id
- How handle token expiration/refresh in BFF?
  - BFF get the access/refresh token cache
  - Check if the access token is invalid, BFF will use the refresh token to create the new access token and update the cache and the auth cookies
- Route-based auth in Ocelot?
  — Put required permission in ocelot.json metadata;
  - Create the custom middleware reads metadata and checks DB before forwarding.
- What is correlation?
  - Is a unique id per request to track the logs end-to-end
  - Gate way for BFF wil generate the ID, and attach into the header X-Correlation-Id: abc123
  - Gateway forward it to the service
  - Every log line will include that id -> easy to debug
- Trace request across services (more)
  Goal: “This user clicked button → which service failed?”
  - Gateway receives request → create correlation id
  - Gateway forwards to Service A/B/C with same id
  - Each service logs with that id
    Always log these fields (structured): CorrelationId, TraceId, UserId, TenantId, RequestPath, StatusCode

# My project

- Using Blazor WASM + BFF:

  - Browser (Blazor WASM) calls the BE (same origin) -> BFF
  - BFF handles logic (OIDC) and keeps tokens server-side

- UI can be served via CDN/static hosting
  - Basically, Blazor WASM publishes to static files (HTML/CSS/JS/WASM/dll in wwwroot). So they can be hosted like a static website and cached on CDN
- The basic flow

```sh
MyApp.sln
 ├─ Client/          (Blazor WebAssembly UI)
 ├─ BffServer/       (ASP.NET Core host + BFF endpoints + Entra login)
 └─ FeedApi/         (Microservice API protected by Entra JWT)
```

- FeedApi (microservice) — validates JWT and returns data

  - FeedApi: the main API server in the microservice, it will validate the token and respond API request

- BffServer — Entra login + cookie session + calls FeedApi with Bearer token

  - Handle login + logout
  - Using TokenAcquisition to extract the token from cookies to call the API from microservice

- Client (Blazor WASM) — calls BFF, redirects to /login when 401
  - /login trigger Entra OIDC
  - Entra will ask user to login and return to /signin-oidc
  - BFF will set the HttpOnly auth cookies

**_Full basic flow_**

- User opens the /feed page -> WASM runs in the browser
- WASM call /bff/feed (WASM call to BFF)
- BFF will check the the user authenticated -> if unauthorized, BFF return 401 -> UI redirects to /login
- /bff/login trigger Entra OIDC -> User will login with Entra and return to /signin-oidc -> then BFF will set the auth cookies
- Then UI call /bff/feed to get data again -> and the cookies will be sent automatically
- BFF get the access from authenticated request from WASM
- BFF call microservice API within the token attach to the Authorization: Bearer header

-> When Logout the UI will goes to Logout page -> WASM call to /bff/logout -> BFF will clear the cookies and call to Entra /sigh-out endpoint

### Flow 1: Login (BFF creates cookie session)

- UI → BFF: user clicks “Login”
- BFF → Entra: redirect to Microsoft login (OIDC)
- Entra → BFF: callback to /signin-oidc
- BFF: validates sign-in, creates session cookie (HttpOnly)
- UI: now authenticated; browser automatically sends cookie on next requests

### - Flow 2: Request Authentication + Authorization (BFF → Ocelot Gateway)

- UI → BFF: calls your BFF API with cookie
- BFF:

  - reads user session
  - gets access token for Gateway (server-side)
  - calls Ocelot Gateway with Authorization: Bearer <token>

- Gateway (AuthN):
  - validates JWT (issuer, signature, expiry, audience)
  - extracts userId from claim (usually oid)
- Gateway (AuthZ):

  - reads required permission from ocelot.json route metadata (ex: SponsorProtocol.Create)
  - queries DB: does userId have this permission?
  - if NO → 403

- Gateway → Microservice:
  - forwards request to internal microservice
  - adds header like X-User-Id: <oid> so microservice can save CreatedBy
