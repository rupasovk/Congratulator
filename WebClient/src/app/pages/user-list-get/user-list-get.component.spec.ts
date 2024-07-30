import { ComponentFixture, TestBed } from '@angular/core/testing';

import { UserListGetComponent } from './user-list-get.component';

describe('UserListGetComponent', () => {
  let component: UserListGetComponent;
  let fixture: ComponentFixture<UserListGetComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [UserListGetComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(UserListGetComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
