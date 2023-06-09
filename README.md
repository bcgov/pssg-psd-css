# pssg-psd-css
PSSG Police Services Division - Community Safety System

[![img](https://img.shields.io/badge/Lifecycle-Stable-97ca00)]

Technology Stack
-----------------

| Layer   | Technology | 
| ------- | ------------ |
| Presentation | Angular 13.3 | Nodejs 14.20 |
| Business Logic | C Sharp - Dotnet Core 3.1 |
| Application Server | Kestrel |
| Data Storage | MS Dynamics On Premise V9 |
| Authentication | N/A |
| Document Storage    | MS SharePoint 2016 |
| Container Platform | OpenShift 4 |
| Zero Trust Security Policy Type | Native Kubernetes |
| Logging | Splunk, Console and Kibana |
| CI/CD Pipeline | Jenkins |


Installation
------------
This application is meant to be deployed to RedHat OpenShift version 4. Full instructions to deploy to OpenShift are in the `openshift` directory.

Developer Prerequisites
-----------------------

**Public Application**
- .Net Core SDK (3.1)
- Node.js version 14.20 
- .NET Core IDE such as Visual Studio or VS Code
- JAG VPN with access to MS Dynamics

**DevOps**
- RedHat OpenShift tools
- Docker
- A familiarity with Jenkins

## Captcha
This system makes use of a Captcha container that handles all aspects of the captcha process used to ensure the applicant is not an automated process.




Contribution
------------

Please report any [issues](https://github.com/bcgov/pssg-psd-csa/issues).

[Pull requests](https://github.com/bcgov/pssg-psd-csa/pulls) are always welcome.

If you would like to contribute, please see our [contributing](CONTRIBUTING.md) guidelines.

Please note that this project is released with a [Contributor Code of Conduct](CODE_OF_CONDUCT.md). By participating in this project you agree to abide by its terms.

License
-------

    Copyright 2021 Province of British Columbia

    Licensed under the Apache License, Version 2.0 (the "License");
    you may not use this file except in compliance with the License.
    You may obtain a copy of the License at 

       http://www.apache.org/licenses/LICENSE-2.0

    Unless required by applicable law or agreed to in writing, software
    distributed under the License is distributed on an "AS IS" BASIS,
    WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
    See the License for the specific language governing permissions and
    limitations under the License.

Maintenance
-----------

This repository is maintained by [BC Attorney General]( https://www2.gov.bc.ca/gov/content/governments/organizational-structure/ministries-organizations/ministries/justice-attorney-general ).
