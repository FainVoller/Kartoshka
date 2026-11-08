// App.jsx
import { useEffect, useRef } from 'react';
import WebViewer from '@pdftron/webviewer';

function App() {
  const viewerRef = useRef(null);

  useEffect(() => {
    WebViewer(
      {
        path: '/lib/webviewer', // путь к скопированным файлам
        initialDoc: '/sample.pdf', // положите файл в public/
      },
      viewerRef.current
    ).then((instance) => {
      // экземпляр viewer готов
      const { docViewer } = instance;
      docViewer.on('documentLoaded', () => {
        console.log('PDF загружен');
      });
    });
  }, []);

  return (
    <div style={{ height: '100vh' }}>
      <div ref={viewerRef} style={{ height: '100%' }} />
    </div>
  );
}

export default App;