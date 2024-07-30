import { ComponentFixture, TestBed } from '@angular/core/testing';

import { WebSocketMessagesComponent } from './web-socket-messages.component';

describe('WebSocketMessagesComponent', () => {
  let component: WebSocketMessagesComponent;
  let fixture: ComponentFixture<WebSocketMessagesComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [WebSocketMessagesComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(WebSocketMessagesComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
