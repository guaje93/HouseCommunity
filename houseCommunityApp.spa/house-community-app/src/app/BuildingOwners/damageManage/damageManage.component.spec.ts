/* tslint:disable:no-unused-variable */
import { async, ComponentFixture, TestBed } from '@angular/core/testing';
import { By } from '@angular/platform-browser';
import { DebugElement } from '@angular/core';

import { DamageManageComponent } from './damageManage.component';

describe('DamageManageComponent', () => {
  let component: DamageManageComponent;
  let fixture: ComponentFixture<DamageManageComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ DamageManageComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(DamageManageComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
