export class DialogSizes {
  static readonly SMALL = new DialogSizes('max-w-lg');
  static readonly MEDIUM = new DialogSizes('max-w-5xl');
  static readonly LARGE = new DialogSizes('max-w-7xl');

  // private to disallow creating other instances of this type
  private constructor(public readonly size: string) {}
}
