### Jasmin Code Sample - Create Item and add a Sales Item extension - C#

This code sample illustrates the use of the Jasmin Web API to create an Item and add a Sales Item extension.
There are two ways of doing this:
#### 1. Creating the item and then add the sales item extension
Using the POST api/{tenantKey}/{orgKey}/salesCore/salesItems/extension.
#### 2. Creating the sales item and the item at the same time.
Using the POST api/{tenantKey}/{orgKey}/salesCore/salesItems