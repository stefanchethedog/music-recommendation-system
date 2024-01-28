import { FC } from "react";
import { SongsForm } from "../../components";

import "./Songs.styles.scss";

const SongsPage: FC = () => {
  return (
    <div className="songs-page">
      <SongsForm />
    </div>
  );
};

export default SongsPage;
