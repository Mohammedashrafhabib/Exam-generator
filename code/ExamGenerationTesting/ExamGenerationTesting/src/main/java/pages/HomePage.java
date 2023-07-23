package pages;

import org.openqa.selenium.By;
import org.openqa.selenium.WebDriver;

public class HomePage 
{
	   By getstartedBtn= By.xpath("//*[@id=\"content\"]/ng-component/div/div/a/div/div");
	   WebDriver driver;
	   public HomePage(WebDriver d)
	   {
		   this.driver=d;
	   }
	   public void clickonbtn() 
	   {
		   driver.findElement(getstartedBtn).click();
	   }
	   
}
