package pages;

import org.openqa.selenium.By;
import org.openqa.selenium.WebDriver;

public class SettingsPage 
{
	   By email= By.xpath("//*[@id=\"content\"]/ng-component/div/form/div[1]/div[2]/div[2]/div[2]/input");
	   By modifybtn= By.xpath("//*[@id=\"content\"]/ng-component/div/form/div[2]/div/div");
	   By passwordbtn=By.xpath("//*[@id=\"content\"]/ng-component/div/form/div[1]/div[1]/a/div");
	   By oldpassword =By.xpath("//*[@id=\"content\"]/ng-component/div/form/div/div[3]/div[1]/input");
	   By newpassword =By.xpath("//*[@id=\"content\"]/ng-component/div/form/div/div[3]/div[2]/input");
	   By confirmpassword =By.xpath("//*[@id=\"content\"]/ng-component/div/form/div/div[3]/div[3]/input");
	   By confirmbtn= By.xpath("//*[@id=\"content\"]/ng-component/div/form/div/div[3]/div[4]/div");
	   WebDriver driver;
	   public SettingsPage(WebDriver d)
	   {
		   this.driver=d;
	   }
	   public void modifyemail(String newemail) 
	   {
		   driver.findElement(email).clear();
		   driver.findElement(email).sendKeys(newemail);
	   }
	   public void clickmodify() 
	   {
		   driver.findElement(modifybtn).click();
	   }
	   public void modifypassword() 
	   {
		   driver.findElement(passwordbtn).click();
	   }
	   public void enteroldpassword(String oldpass) 
	   {
		   driver.findElement(oldpassword).sendKeys(oldpass);
	   }
	   public void enternewpassword(String newpass) 
	   {
		   driver.findElement(newpassword).sendKeys(newpass);
	   }
	   public void enterconfirmpassword(String confirmpass) 
	   {
		   driver.findElement(confirmpassword).sendKeys(confirmpass);
	   }
	   public void confirm() 
	   {
		   driver.findElement(confirmbtn).click();
	   }
	   

}
