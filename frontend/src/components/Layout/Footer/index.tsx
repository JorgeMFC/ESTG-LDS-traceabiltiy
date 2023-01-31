import { FooterCopyright } from 'flowbite-react/lib/esm/components/Footer/FooterCopyright';

export default function Footer() {
  return (
    <footer className="bg-white shadow-footer px-6 py-8">
      <FooterCopyright
        by="LDS2223_03"
        year={2023}
        href=""
        className="block text-sm text-gray-400 text-center"
      />
    </footer>
  );
}
