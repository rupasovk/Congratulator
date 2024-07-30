import { Component, Input, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators, ReactiveFormsModule } from '@angular/forms';

import { HttpClient, HttpEventType } from '@angular/common/http';
import { catchError, finalize, tap } from 'rxjs/operators';
import { Subscription, throwError } from 'rxjs';
import { HttpClientModule } from '@angular/common/http';
import { CommonModule } from '@angular/common';
import { User } from '../../model/user.model';
import { UserImage } from '../../model/userImage.model';

@Component({
  selector: 'app-user-post',
  standalone: true,
  imports: [ReactiveFormsModule, HttpClientModule, CommonModule],
  templateUrl: './user-post.component.html',
  styleUrls: ['./user-post.component.css']
})
export class UserPostComponent implements OnInit {
  registerForm!: FormGroup;
  isLoading: boolean = false;
  error: string | null = null;
  selectedPhoto: File | null = null;
  user = new User();
  userImage = new UserImage();

  @Input()
  requiredFileType:string | undefined;

  fileName = '';
  uploadProgress:number | undefined;
  uploadSub: Subscription | undefined;
  file: File | undefined
  formData: FormData | undefined
  
  constructor(private formBuilder: FormBuilder, private http: HttpClient) { }

  ngOnInit() {
    this.initForm();
    this.formData = new FormData();
  }

  initForm() {
    this.registerForm = this.formBuilder.group({
      name: ['', Validators.required],
      surname: ['', Validators.required],
      birthday: ['', Validators.required],
      email: ['', [Validators.required, Validators.email]],
      country: ['', Validators.required],
      image: [null, Validators.required]
    });
  }

  onSubmit() {  
    const formValue = this.registerForm.value;
    console.log(formValue);
    //const reader = new FileReader();
    //reader.onload = () => {
    //  this.userImage.ImageBytes = new Uint8Array(reader.result as ArrayBuffer);
    //};
    //if (this.selectedPhoto) {
    //  reader.readAsArrayBuffer(this.selectedPhoto);
    //}

    //const reader = new FileReader();
    //reader.onload = () => {
    //  // Преобразуем результат в base64-строку
    //  this.user.UserImage = btoa(Array.from(new Uint8Array(reader.result as ArrayBuffer)).map(byte => String.fromCharCode(byte)).join(''));
    //  console.log(this.user.UserImage);
    //};
    this.user.UserName = formValue.name;
    this.user.Surname = formValue.surname;
    this.user.Birthday = formValue.birthday;
    this.user.Email = formValue.email;
    this.user.Country = formValue.country;
    console.log(this.user)
    //this.user.UserImage = this.userImage.ImageBytes;

    //this.userImage.User = this.user;
    if (this.formData)
      this.formData.append("UserDto", JSON.stringify(this.user));


    if (formValue && this.registerForm.valid) {
      this.isLoading = true;
      this.http.post('https://localhost:7267/api/UserAPI', this.formData//this.user
      //{
      //  name: formValue.name,
      //  surname: formValue.surname,
      //  birthday: formValue.birthday,//.toISOString(), // Преобразуйте дату в ISO-формат
      //  email: formValue.email,
      //  country: formValue.country,
      //  UserImage: this.user.UserImage
      // }
      )
        .pipe(          
          tap(() => {
            // Обработка успешной отправки
            console.log('Form submitted successfully');
          }),
          catchError((error) => {
            this.error = 'Ошибка при отправке формы. Пожалуйста, попробуйте еще раз.';
            return throwError(error);
          }),
          finalize(() => {
            this.isLoading = false;
          })
        )
        .subscribe();
    } else {
      this.error = 'Пожалуйста, заполните все обязательные поля.';
    }
  }

  onPhotoSelected(event: Event) {
    const fileInput = event.target as HTMLInputElement;
    if (fileInput.files && fileInput.files.length > 0) {
      this.selectedPhoto = fileInput.files[0];
    } else {
      this.selectedPhoto = null;
    }
  }

  onFileSelected(event: { target: { files: File[]; }; }) {
    this.file = event.target.files[0];
  
    if (this.file ) {
      this.fileName = this.file.name;
      if (this.formData)
        this.formData.append("UserImage", this.file);

      //const upload$ = this.http.post("/api/thumbnail-upload", formData, {
      //    reportProgress: true,
      //    observe: 'events'
      //})
      //.pipe(
      //    finalize(() => this.reset())
      //);
    
      //this.uploadSub = upload$.subscribe(event => {
      //  if (event.type == HttpEventType.UploadProgress && event.total) {
      //    this.uploadProgress = Math.round(100 * (event.loaded / event.total));
      //  }
      //})
    }    
  }

  cancelUpload() {
    if (this.uploadSub)
      this.uploadSub.unsubscribe();
    this.reset();
  }

  reset() {
    this.uploadProgress = undefined;
    this.uploadSub = undefined;
  }
}