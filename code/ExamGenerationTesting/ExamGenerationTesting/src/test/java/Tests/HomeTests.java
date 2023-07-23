package Tests;

import org.openqa.selenium.By;
import org.openqa.selenium.JavascriptExecutor;
import org.openqa.selenium.WebDriver;
import org.openqa.selenium.chrome.ChromeDriver;
import org.testng.annotations.AfterMethod;
import org.testng.annotations.BeforeMethod;
import org.testng.annotations.Test;

import dev.failsafe.internal.util.Assert;
import pages.HomePage;
import pages.LoginPage;

public class HomeTests {
	WebDriver driver;
    @BeforeMethod
    public void setup() throws InterruptedException
    {
    	driver =new ChromeDriver();
    	driver.get("http://41.38.127.223:2999");
    	driver.manage().window().maximize();
    	JavascriptExecutor js = (JavascriptExecutor) driver;  
        js.executeScript("window.scrollBy(0,1000)");
        LoginPage login=new LoginPage(driver);
        login.Enteremail("admin");
        login.Enterpassword("P@ssw0rd");
        login.Login();
        Thread.sleep(1000);
    }
    @Test 
    public void openstartpage() throws InterruptedException 
    {
    	HomePage home=new HomePage(driver);
    	home.clickonbtn();
    	Thread.sleep(1000);
    	String txt=driver.findElement(By.xpath("//*[@id=\"content\"]/ng-component/div/div[1]/h1")).getText();
    	Assert.isTrue(txt.equalsIgnoreCase("ONLY THREE STEPS TO SEE THE MAGIC"),"invalid redirction");
    }
    @AfterMethod
    public void quit()
    {
    	driver.quit();
    }
   
}
