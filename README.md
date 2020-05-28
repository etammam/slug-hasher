# Slug Hasher
hashing the url paramters on asp.net mvc arch
### Usage
To encrypt some value and pass it to another action in MVC you would do something like the below.
```cs
[AcceptVerbs(HttpVerbs.Post)]
public ActionResult Index(FormCollection collection)
{
    SecureQueryString qs = new SecureQueryString(mKey);

    qs('YourName') = collection('name');
    qs.ExpireTime = DateTime.Now.AddMinutes(2);

    Response.Redirect('Home.aspx/About?data=' + HttpUtility.UrlEncode(qs.ToString()));
}
```
In the action that we redirect to, you would need to have this same key and the query string value itself to decrypt it. Keep in mind that if you don't have the correct key or if you try to decrypt the value after the expiration, the class will throw an exception.
```cs
public ActionResult About()
{
    if (Request('data') != null) {
        try {
            SecureQueryString qs = new SecureQueryString(mKey, Request('data'));

            ViewData('Message') = 'Your name is ' + qs('YourName');
        }
        catch (Exception ex) {

        }
    }
    return View();
}

```
&copy; Eslam M. Tammam
