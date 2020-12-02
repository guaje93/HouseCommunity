/* tslint:disable:no-unused-variable */
import { async, ComponentFixture, TestBed } from '@angular/core/testing';
import { By } from '@angular/platform-browser';
import { DebugElement } from '@angular/core';

import { DamageComponent } from './Damage.component';

describe('DamageComponent', () => {
  let component: DamageComponent;
  let fixture: ComponentFixture<DamageComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ DamageComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(DamageComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
