export class TableSizes {
  static readonly SMALL = new TableSizes('w-[56rem]');
  static readonly MEDIUM = new TableSizes('w-[80rem]');
  static readonly LARGE = new TableSizes('w-full');

  // private to disallow creating other instances of this type
  private constructor(public readonly size: string) {}
}
