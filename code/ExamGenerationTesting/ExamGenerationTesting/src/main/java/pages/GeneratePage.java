package pages;

import org.openqa.selenium.By;
import org.openqa.selenium.WebDriver;

public class GeneratePage 
{  
	   By input= By.xpath("//*[@id=\"content\"]/ng-component/div/form/div/div/div/div/div/textarea");
	   By generateBtn= By.xpath("//*[@id=\"content\"]/ng-component/div/form/div/a/div/div[2]/div");
       By WHcheckbox= By.xpath("//*[@id=\"WH\"]");
       By MCQcheckbox= By.xpath("//*[@id=\"MCQ\"]");
       By Completecheckbox= By.xpath("//*[@id=\"COMPLETE\"]");
       By T_Fcheckbox= By.xpath("//*[@id=\"T_F\"]");
	   WebDriver driver;
	   public GeneratePage(WebDriver d)
	   {
		   this.driver=d;
	   }
	   public void entercontext(String context) 
	   {
		   driver.findElement(input).sendKeys(context);
	   }
	   public void generate() 
	   {
		   driver.findElement(generateBtn).click();
	   }

           public void T_Fcheckboxclick() 
	   {
		   driver.findElement(T_Fcheckbox).click();
	   }
           public void MCQcheckboxclick() 
	   {
		   driver.findElement(MCQcheckbox).click();
	   }
           public void WHcheckboxclick() 
	   {
		   driver.findElement(WHcheckbox).click();
	   }
           public void Completecheckclick() 
	   {
		   driver.findElement(Completecheckbox).click();
	   }
           public void allcheckboxs(int mask) 
   	    {
   		    if(mask==(1<<0))
   		    {
   		    	T_Fcheckboxclick();
   		    }
   		    else if(mask==(1<<1))
   		    {
   		    	MCQcheckboxclick();
   		    }else if(mask==(1<<2))
   		    {
   		    	WHcheckboxclick();
   		    }
   		    else if(mask==(1<<3))
   		    {
   		    	Completecheckclick();
   		    }
   		}

}
