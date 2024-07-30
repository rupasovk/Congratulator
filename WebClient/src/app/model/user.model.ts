import { UserImage } from "./userImage.model";

export class User {
    Id: number | undefined;
    UserName: string | undefined;
    Surname: string | undefined;
    Birthday: Date | undefined;
    Email: string | undefined;
    Country: string | undefined;
    UserImage: string | undefined
  
    constructor(data?: any) {
      if (data) {
        this.Id = data.Id;
        this.UserName = data.Name;
        this.Surname = data.SurName;
        this.Birthday = new Date(data.BirthDay);
        this.Email = data.Email;
        this.Country = data.Country;
        this.UserImage = data.UserImage;
      }
    }
  }