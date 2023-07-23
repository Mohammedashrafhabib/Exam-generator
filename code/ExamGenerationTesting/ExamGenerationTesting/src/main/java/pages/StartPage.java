package pages;

import org.openqa.selenium.By;
import org.openqa.selenium.WebDriver;

public class StartPage 
{
	   By getstartedBtn= By.xpath("//*[@id=\"content\"]/ng-component/div/div[1]/div[3]");
	   WebDriver driver;
	   public StartPage(WebDriver d)
	   {
		   this.driver=d;
	   }
	   public void clickonbtn() 
	   {
		   driver.findElement(getstartedBtn).click();
	   }

}
