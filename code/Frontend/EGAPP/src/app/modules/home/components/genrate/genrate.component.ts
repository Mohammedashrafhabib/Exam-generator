import { contextModel } from './../../models/context.model';
import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { GenerateService } from '../../services/generate.service';
import { ResultModel } from '../../models/result.model';
import { TranslateService } from '@ngx-translate/core';
import { NotificationService } from 'src/app/sharedFeatures/services/notification.service';

@Component({
  templateUrl: './genrate.component.html',
  styleUrls: ['./genrate.component.scss']
})
export class GenrateComponent implements OnInit {
  genrateForm!: FormGroup;
  contextModel:contextModel=new contextModel();
  isloading:boolean=false;
  questions:ResultModel;



  constructor(    private fb: FormBuilder,
    private notificationService: NotificationService,
    private translateService: TranslateService,

    private service:GenerateService
    ) { }

  ngOnInit(): void {
    this.buildform();
  }
  buildform(){
    this.genrateForm= this.fb.group({

      context:[null, [Validators.required, Validators.maxLength(10000)]],
      MCQ:[false, []],
      COMPLETE:[false, []],
      WH:[false, []],
      T_F:[false, []],

    })
  }
  submit(){
    debugger;
    this.isloading=true;
    if(this.genrateForm.valid){

       this.contextModel.context= this.genrateForm?.controls['context'].value;
       this.contextModel.MCQ= this.genrateForm?.controls['MCQ'].value;
       this.contextModel.COMPLETE=this.genrateForm?.controls['COMPLETE'].value;
       this.contextModel.WH= this.genrateForm?.controls['WH'].value;
       this.contextModel.T_F= this.genrateForm?.controls['T_F'].value;

       this.service.genrate(this.contextModel).subscribe(
         res=>{
          this.questions=res;
          this.isloading=false;

           console.log(res);
         },
         (error: any) => {
          this.isloading=false;

          if (error.error.code == 400) {
            let key = 'error.400';
            this.translateService.get([key]).subscribe(res => {
              this.notificationService.showErrorTranslated(`${res[key]}`, '');
            });
          }
          if (error.error.code == 500) {
            let key = 'error.500';
            this.translateService.get([key]).subscribe(res => {
              this.notificationService.showErrorTranslated(`${res[key]}`, '');
            });
          }
        }
       )

    }
    else{
      this.notificationService.showError('please enter a context',"");

    }
  }
}
