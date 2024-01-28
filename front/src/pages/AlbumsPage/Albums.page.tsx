import { FC } from "react";
import { AlbumsForm } from "../../components";

import "./Albums.styles.scss";

const AlbumsPage: FC = () => {
  return (
    <div className="albums-page">
      <AlbumsForm />
    </div>
  );
};

export default AlbumsPage;
