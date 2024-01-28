import { FC } from "react";
import { ArtistForm } from "../../components";

import "./Artist.styles.scss";

const ArtistPage: FC = () => {
  return (
    <div className="artist-page">
      <ArtistForm />
    </div>
  );
};

export default ArtistPage;
