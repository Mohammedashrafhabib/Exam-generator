package Tests;

import org.openqa.selenium.By;

import org.openqa.selenium.JavascriptExecutor;
import org.openqa.selenium.WebDriver;
import org.openqa.selenium.chrome.ChromeDriver;
import org.testng.annotations.AfterMethod;
import org.testng.annotations.BeforeMethod;
import org.testng.annotations.Parameters;
import org.testng.annotations.Test;

import dev.failsafe.internal.util.Assert;
import pages.HomePage;
import pages.LoginPage;
import pages.StartPage;
import pages.GeneratePage;
public class GenerateTests 
{
	WebDriver driver;
    @BeforeMethod
    public void setup() throws InterruptedException
    {
    	driver =new ChromeDriver();
    	driver.get("http://41.38.127.223:2999/home");
    	driver.manage().window().maximize();
    	JavascriptExecutor js = (JavascriptExecutor) driver;  
        js.executeScript("window.scrollBy(0,1000)");
        LoginPage login=new LoginPage(driver);
        login.Enteremail("admin");
        login.Enterpassword("P@ssw0rd");
        login.Login();
        Thread.sleep(1000);
        HomePage home=new HomePage(driver);
        home.clickonbtn();
        Thread.sleep(1000);
        StartPage start=new StartPage(driver);
        start.clickonbtn();
        Thread.sleep(1000); 
        js.executeScript("window.scrollBy(0,1000)");
    }
    @Test 
    @Parameters ({"context","mask"})  
    public void generateQA(String context,String mask) throws InterruptedException 
    {
    	GeneratePage generatePage=new GeneratePage(driver);
    	generatePage.entercontext(context);
    	Thread.sleep(3000);
    	int Mask=Integer.parseInt(mask);
    	for(int i=0;i<4;i++)
    	{
    		 generatePage.allcheckboxs(Mask&(1<<i));
    	}
    	Thread.sleep(2000);
    	generatePage.generate();
    	Thread.sleep(5*60*1000);
    	String txt= driver.findElement(By.xpath("//*[@id=\"content\"]/ng-component/div/form/div/div/div/div[2]/div[1]/div/textarea")).getText();
    	Assert.isTrue(!(txt.equalsIgnoreCase("")),"invalid Generation");
    }
   /* @Test 
    @Parameters ({"context"})
    public void generateallQA(String context,String mask) throws InterruptedException 
    {
    	// this text to test all combination of all checkboxes do it take a lot of time (75)minute
    	GeneratePage generatePage=new GeneratePage(driver);
    	generatePage.entercontext(context);
    	Thread.sleep(3000);
    	int Mask=Integer.parseInt(mask);
    	int co=0;
    	for(int j=1;j<16;j++) 
    	{
	    	for(int i=0;i<4;i++)
	    	{
	    		 generatePage.allcheckboxs(Mask&(1<<i));
	    	}
	    	generatePage.generate();
	    	Thread.sleep(5*60*1000);
	    	String txt= driver.findElement(By.xpath("//*[@id=\"content\"]/ng-component/div/form/div/div/div/div[2]/div[1]/div/textarea")).getText();
	    	if(!txt.equalsIgnoreCase(""))
	    	{
	    		co++;
	    	}
    	}
    	Assert.isTrue(co==15,"invalid Generation");
    }*/
    @AfterMethod
    public void quit()
    {
    	driver.quit();
    }
}
