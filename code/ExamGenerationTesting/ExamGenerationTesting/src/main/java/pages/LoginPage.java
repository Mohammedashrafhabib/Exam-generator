package pages;
import org.openqa.selenium.By;
import org.openqa.selenium.WebDriver;
public class LoginPage {
   By emailInput= By.xpath("//*[@id=\"loginEmail\"]");
   By passwordInput= By.xpath("//*[@id=\"loginPassword\"]");
   By loginBtn= By.xpath("/html/body/app-root/div/app-login/section/div/form/div/div[5]/div[1]/div/div");
   By googleBtn= By.xpath("/html/body/app-root/div/app-login/section/div/form/div/div[1]/a[1]/img");
   By facebookBtn = By.xpath("/html/body/app-root/div/app-login/section/div/form/div/div[1]/a[2]/img");
   WebDriver driver;
   public LoginPage(WebDriver d)
   {
	   this.driver=d;
   }
   public void Enteremail(String email)
   {
	   driver.findElement(emailInput).sendKeys(email);
   }
   public void Enterpassword(String password)
   {
	   driver.findElement(passwordInput).sendKeys(password);
   }
   public void Login()
   {
	   driver.findElement(loginBtn).click();
   }
   public void opengoogle() 
   {
	  driver.findElement(googleBtn).click();
   }
   public void openfacebook() 
   {
	  driver.findElement(facebookBtn).click();
   }
}
