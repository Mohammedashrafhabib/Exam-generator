package pages;

import org.openqa.selenium.By;
import org.openqa.selenium.WebDriver;

public class SignupPage 
{
	   By firstname= By.xpath("/html/body/app-root/div/app-login/section/section/div/form/div/div[3]/div[1]/input");
	   By secondname= By.xpath("/html/body/app-root/div/app-login/section/section/div/form/div/div[3]/div[2]/input");
	   By email= By.xpath("/html/body/app-root/div/app-login/section/section/div/form/div/div[4]/input");
	   By loginBtn= By.xpath("/html/body/app-root/div/app-login/section/section/div/form/div/div[6]/p/span[2]");
	   By continuebtn= By.xpath("/html/body/app-root/div/app-login/section/section/div/form/div/div[5]/div/div");
	   WebDriver driver;
	   public SignupPage(WebDriver d)
	   {
		   this.driver=d;
	   }
	   public void Enteremail(String em)
	   {
		   driver.findElement(email).sendKeys(em);
	   }
	   public void Enterfirstname(String name)
	   {
		   driver.findElement(firstname).sendKeys(name);
	   }
	   public void Entersecondname(String name)
	   {
		   driver.findElement(secondname).sendKeys(name);
	   }
	   public void clickcontinue()
	   {
		   driver.findElement(continuebtn).click();
	   }
	   public void Login()
	   {
		   driver.findElement(loginBtn).click();
	   }

}
