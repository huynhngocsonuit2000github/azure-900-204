# Overview

- Sponsor send protocol -> Bản kế hoạch của study
- Staff read the protocol and use Solar to enter/config the data for Lab rely on those information
- Solar create Study Specification (study spec) -> tài liệu mô tả cách mình đã setup
- Send study spec for Sponsor to sign off

## The main responsibilities in Solar:

- Based on protocol, team will config the study spec in Solar
- Study spec includes:
  - Schedule of patient visits
  - Tests required at each visit
  - Study rules (adverse reactions, panic values)
  - Site and country information
  - Sponsor-specific requirement
- After done the study spec
  - Export the PDF and send to sponsor
  - Sponsor will comment and team will edit the config
  - Send to DocuSign to sign off
    -> Study spec done once sponsor sign

# ICOLIMS Client

- The lab study setup team use, desktop client based
- Config ICOLIMS db for the study
  - Setup required tests
  - Config visit schedules
  - Apply rules related to test result
- 30-40% auto, the rest is manual
  - The user reads the study spec (PDF from Solar)
  - Then manually config the db in icolims to match it
- There is a manual QC step
  - The icolims setup is checked against the Solar study spec
- Once complete:
  - DB is set live
  - Sites perform test
  - Labs enter test result into the icolims db
  - Result can be viewed in the system

# Current Process of Labs study setup

## Step 1: input

- Sponsor send protocol including all the **rule** for study: rule, schedule, required tests, site country information, specific requirement from sponsor

## Step 2: Study spec creation

- Pull existing icolims data (test codes, country codes, specimen transportation data, kit component data) into solar
- Study setup team (team) config the study spec in Solar
- Sponsor reviews -> feedback -> update
- Final version is signed off

## Step 3: Database Configuration in ICOLIMS

- Based on Study spec (PDF from Solar)
  - Some data is auto-push from Solar to ICOLIMS
  - Most configuration is done manually in ICOLIMS client
- Manual QC is performed comparing
  - Solar study spec
  - ICOLIMS db configuration

## Step 4: go live

- db is made available
- Site start performing tests
- Test result flow back into icolims

# Problems

- ICOLIMS ls the legacy desktop system
- Highly manual db configuration
- Two separate systems
- Manual QC
- Sponsor data collection is unstructured
  - the information is gathered via emails and meetings
  - No standard collection form
    => The system is separated and so many steps, easy to make mistake, waist time because of two separated systems, which is no integration well

===========

# Expected

build the single web-based Study Setup Portal to replace the old process

- Replace Solar
  - Manage the study spec on the web
  - Generate the report file study spec and send to sponsor
- Reduce dependency on the icolims desktop client
  - users should do db configuration in the web portal
  - no need to manually enter everything in icolims
- Increase data automation
  - In the future phase: AI tools will extract data from the protocol
  - Prefill data from
    - data from icolims
    - proposal
    - information provided from sponsor
- Sponsor data collection form
  - Send form to sponsor -> sponsor fill -> system auto fill to study spec
- Dynamic logic in the study spec
  - Sponsor-specific rules
  - Study requirements
- Automated QC process
  - User can run the QC task
  - System auto validate against all the business-defined rules
- Single interface
  - Use 1 web UI for all

========
