Ensure this file is marked Do Not Copy in the Project

TENANT IDS
==========

Each Tenant ID is a guid. These IDs grant authenticated access to RT License. 
As some of these point to Live accounts it is vital you keep the guid secret. 
The guid is used in the URL so it could be seen by someone e.g. watching your screen in a demo.
IF you think you might have accidently shown one then contact Jay to get it changed.


I have prepared Tenants for localhost development, Dev, Release, QA, and Live.
You can edit, or add new tenants, to localhost, Dev, and Release freely.
Please do not edit or add to QA and Live without checking with Jay first.

There are also some in use here for testing using the Samples projects. Not listed here. See the source!


localhost
---------
Note: 
- You will need to use the "Local" setting in RTLicense
- You will need to be running a localhost copy of StubIdp from the Saml2 repository (this project)
#1
6be75424-ff6a-486a-b694-a11549d8136e.json
https://localhost:44300/6be75424-ff6a-486a-b694-a11549d8136e
#2
4db9c0a1-8afe-4fd4-b6cd-ba6584c13860.json
https://localhost:44300/4db9c0a1-8afe-4fd4-b6cd-ba6584c13860

Dev
---
#1 - Logs in to ClientID 30177 on license-dev.rightstracker.com
ce6c26a3-cb76-42fe-b03d-6af8c63d0fb1.json
https://saml2-stubidp.azurewebsites.net/ce6c26a3-cb76-42fe-b03d-6af8c63d0fb1
#2 - Logs in to ClientID 30177 on LOCALHOST
(added as a convenience for developers who don't want to switch to 'Local' settings)
7533cd5c-99c4-4790-9f09-712b23b4b770.json
https://localhost:44300/7533cd5c-99c4-4790-9f09-712b23b4b770

Release
-------
#1 - Logs in to ClientID 30177 on license-release.rightstracker.com
945d6909-e88d-4171-bca4-c2a2d521a5fb.json
https://saml2-stubidp.azurewebsites.net/945d6909-e88d-4171-bca4-c2a2d521a5fb

QA
--
#1 - Logs in to ClientID 950 (our demo/test account) on license-qa.rightstracker.com
839f5344-25dc-498d-883f-2bde0f8e7c40.json
https://saml2-stubidp.azurewebsites.net/839f5344-25dc-498d-883f-2bde0f8e7c40
#2 - EBU Alternative IdP   * * * MASSIVE SECURITY RISK!!! * * * (so it might not be there) - TODO: disable once we are very happy with it
This is a test! Not sure if it will work, or if it will break their end!
But... keen to find out!
943afd4c-c87d-488f-858e-3e409d4e9210.json
https://saml2-stubidp.azurewebsites.net/943afd4c-c87d-488f-858e-3e409d4e9210

LIVE
----
#1 - Logs in to ClientID 950 (our demo/test account) on license.rightstracker.com
fc4fdb92-cb8e-4307-8939-06b01d835ffd.json
https://saml2-stubidp.azurewebsites.net/fc4fdb92-cb8e-4307-8939-06b01d835ffd
#2 - EBU Alternative? ... prefer not to, but we might want it at first, especially if we have any support issues
TODO:

