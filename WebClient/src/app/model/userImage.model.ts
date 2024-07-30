import { User } from "./user.model";

export class UserImage {
    ImageBytes: string | undefined;
  
    constructor(data?: any) {
      if (data) {
        this.ImageBytes = data.Image;
      }
    }
  }