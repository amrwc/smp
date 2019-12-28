export class Error {
  constructor(key: string, value: string) {
    this.key = key;
    this.value = value;
  }

  public key: string;
  public value: string;
}
