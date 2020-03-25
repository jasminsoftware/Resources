### Jasmin Integration Code Sample - C#
 This code sample is a command line application that illustrates the use of the Jasmin Web API to:

* Authenticate using Client Credentials OAuth flow
* Use of OData to get entities using sample filter
* Create, Update and Delete different entity "types" (master, detail, base and extensions)

#### Getting Started

All you need to do is run the project using Visual Studio 2017.

#### Prerequisites

In order to be all to run tha sample you'll need to have :
* Visual Studio 
* Nuget
* A clientid and clientsecret (registered application)
* A JASMIN account and subscription (sample DEMO subscription will do).

#### Running the sample

You can either built the project and run it from the command prompt on you bin folder
```
jasmin.integrationsample
```
or run it on debug.

While running the application this way you will be prompted to input all the parameters needed for the integration, your:
* CliendId 
* ClientSecret
* AccountKey
* SubscriptionKey

#### Running the sample (the smart way)
#### Command Line parameters

This sample provides some command line parameters in order to help will you be more productive 

```
/h                  - Displays a small help information describing all the
                      remainig parameters
```

The remaining parameters
```
/ClientId           - Your app's client id
                      OPTIONAL: The program will request for it's input if argument is not set.

/ClientSecret       - Your app's client secret"
                      OPTIONAL: The program will request for it's input if argument is not set.

/AccountKey         - The AccountKey of your subscription
                      OPTIONAL: The program will request for it's input if argument is not set.

/SubscriptionKey    - The SubscriptionKey of your subscription
                      OPTIONAL: The program will request for it's input if argument is not set.
```

#### Running the sample (take2)
We this in mind, you can know go back to runing this sample and either:

* While on Visual Studio debug, add the parameters to the start options -> command line arguments the project, like this:
```
 /ClientId:myprecisousapp /ClientSecret:mysecretpass /AccountKey:100000 /SubscriptionKey:100000-0000
```
* At the command prompt passing all the parameters
```
jasmin.integrationsample /ClientId:myprecisousapp /ClientSecret:mysecretpass /AccountKey:100000 /SubscriptionKey:100000-0000
```

 In either cases this will prompt you right to the application menu.

#### Looking under the wood

Has you run the application it will perform the folowing steps:

```
1. The application will start by request the access token using your client credentials (CliendId, ClientSecret)

2. If you have multiple companies on your subscription (for this sample purpose only) we will select the first company using OData

3. For simplification, we will set logistics settings to allow negative stock.

4. The application menu will be displayed and you can start your integration test.
```

#### Play along with your options

 The application menu will look something like this:
```
1 - Customers
2 - Sales Items
3 - Sales Orders
4 - Sales Invoices

0 - Exit
```

If you press '1', the **Customer** submenu will display this options:
```
1 - Create Customer and Party (AllInOne)
2 - Create Party and then Customer Extension
3 - Get Last Created Customer
4 - Update Last Customer City
5 - Update Last Customer PaymentTerm
6 - Delete Last Customer Extension
7 - Add Customer Extension
8 - Delete All Parties Created on Sample

0 - Back to Main Menu

```
**TIP** - Go through all options (go back to option 3 whenever you want and see how the application handles) before returning to the main menu.

If you press '2', the **Sales Items** submenu will display this options:
```
1 - Create Sales Item and Item (AllInOne)
2 - Create Item and then Sales Item Extension
3 - Get Last Sales Item Created
4 - Set a Price
5 - Update that Price
6 - Delete Sales Item Extension
7 - Add Sales Item Extension
8 - Delete All Items Created on Sample

0 - Back to Main Menu
```
**TIP** - Go through all options (go back to option 3 whenever you want and see how the application handles) before returning to the main menu.

If you press '3', the **Sales Orders** SubMenu will look like this:
```
1 - List First page of 20
2 - List filtered using Odata
3 - Create Order
4 - Get LastCreated Order
5 - Insert New Line on Order
6 - Add Quantity (+2) on Order
7 - Delete LastLine on Order
8 - Delete Last Order

0 - Back To Main Menu
```

**TIP** - I would suggest you to go from option 1 to 4, then play a bit with options 5, 6 and 7 (go back to option 4 as many times as you want to get a view of the progress of your actions) and then option 8 and 0 to exit.

If you press '4', the **Sales Invoices** submenu will display this options:
```
1 - List First page of 20
2 - List filtered using Odata
3 - Create Invoice
4 - Get LastCreated Invoice
5 - Delete Last Invoice

0 - Back To Main Menu
```

**TIP** - Go through all options before returning to the main menu.

*Have a great time and think about that great idea that will make you stand-up from the rest of the crowd!*

This sample should help you quickly getting started on building your own JASMINE integration!

