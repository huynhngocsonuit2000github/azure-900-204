# Core feature

- Process management
  - study/site management, monitoring, regulatory tracking, reporting
- Approval workflow
- UAM: one unified system used by everyone (same data, same process, standard way of working)
- Audit trail
- Dynamic Reporting
- E-Signature
- ECDMS Integration to reduce manual data entry
- Authentication + Authorization + Azure
- Email scheduler + Notification

====

# Definition

- CRA → the person who checks the site for the sponsor
- Visit → the event where that check happens
- Relationship → Visit connects Study + Site + CRA (+ site staff)

====

# Core entity

- Study - top-level trial record (protocol, indication, phase, milestones).
  - group everything: all sites, all monitoring visit => to see overall status
- Sites - locations running the study; each Study can have many Sites.
  - A Site tracks the progress and performance of a study at a specific address.
- Investigators / Site Staff - stored as Contacts linked to Sites.
  - Contact entity: present for the person (PI (Principal-Investigator) → main doctor in charge, Sub-investigator, Study coordinator, Pharmacist) who is performing the work on the study for that site
    - Contact will store: Name, Role in the study, Email / phone, Which site and study they work on => Know who to call or email => Know who is responsible at each site => Send visit confirmations, questions, documents to the right person => **"Contacts = the actual people responsible for the study at each site, with their roles and contact details."**
- Site Visits - monitoring visits per Site and Study, with reports, checklists, and follow-up items.
- All of these are stored together in CTMS, making it the central source of truth for operational trial data.

```
Study
  └── Site
        ├── Site Contacts
        └── Visits
              ├── CRA
              └── Visit Report

```

====

# Study team decides: “Berlin site needs monitoring”

- They look at things like:
  Has the site started recruiting?
  How many patients enrolled?
  How long since the last visit?
  Any data problems?
- Based on the study plan or risk, they say:
  “This site must be checked now.”
  🧠 This is planning, not execution.

# Some core from confluence

- User login + role base access
- Management
  - Study
  - Contact
  - Site
  - Visit
- Simple report/list screens
- Upload/download document
- Complex workflow (multi-step approvals, e-signature)

## Study

- one clinical trial - the project

## Site

- a physical place where the trial runs (hospital/clinic)

## Site Visit

- records monitoring visit done by CRA at a Site

# Minimal user flow (what a user should be able to do)

![alt text](./imgs/image.png)
![alt text](./imgs/image-1.png)
![alt text](./imgs/image-2.png)

## Some flow we can implement the project

1. Login with basic user role
2. Study list / detail
3. Audit feature for all the entity
4. Site list / detail
5. Site Visit list / detail
6. Document upload modal / page
7. Simple Report
   - upcoming Visits: table: study, site, visit type, planned date
   - Recently completed visits
8. Dynamic report feature
