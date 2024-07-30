import { ComponentFixture, TestBed } from '@angular/core/testing';

import { UserBirthdaysComponent } from './user-birthdays.component';

describe('UserBirthdaysComponent', () => {
  let component: UserBirthdaysComponent;
  let fixture: ComponentFixture<UserBirthdaysComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [UserBirthdaysComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(UserBirthdaysComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
