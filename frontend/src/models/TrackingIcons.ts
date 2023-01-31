import { IconType } from 'react-icons';
import { FaTruck, FaCheck, FaHammer } from 'react-icons/fa';
import { ImCross } from 'react-icons/im';
import { FiLoader } from 'react-icons/fi';

export class TrackingIcons {
  static readonly WatingList = new TrackingIcons(FiLoader, 'bg-purple-700');
  static readonly InProgress = new TrackingIcons(FaHammer, 'bg-blue-600');
  static readonly Shipping = new TrackingIcons(FaTruck, 'bg-orange-500');
  static readonly Finished = new TrackingIcons(FaCheck, 'bg-green-500');
  static readonly Canceled = new TrackingIcons(ImCross, 'bg-red-600');

  // private to disallow creating other instances of this type
  private constructor(
    public readonly icon: IconType,
    public readonly color: string
  ) {}
}
