import { ComponentFixture, TestBed } from '@angular/core/testing';

import { GenrateComponent } from './genrate.component';

describe('GenrateComponent', () => {
  let component: GenrateComponent;
  let fixture: ComponentFixture<GenrateComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ GenrateComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(GenrateComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
